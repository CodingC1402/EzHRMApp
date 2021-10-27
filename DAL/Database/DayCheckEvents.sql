DELIMITER $

DROP EVENT IF EXISTS StartBusinessHoursEvent;
CREATE EVENT StartBusinessHoursEvent
ON SCHEDULE AT (
    CURRENT_DATE() + INTERVAL 1 DAY + INTERVAL 6 HOUR
    )
ON COMPLETION NOT PRESERVE
DO
    BEGIN
        DECLARE gioTanLamHomNay datetime;

        /* check if business is currently having a holiday */
        CREATE TEMPORARY TABLE holidays
        SELECT *
        FROM nghile
        WHERE Ngay <= DAY(CURRENT_DATE() - INTERVAL 1 DAY)
        AND Thang <= MONTH(CURRENT_DATE() - INTERVAL 1 DAY)
        AND CURRENT_DATE() - INTERVAL 1 DAY <= MAKEDATE(YEAR(CURRENT_DATE()), 1)
            + INTERVAL Thang - 1 MONTH
            + INTERVAL Ngay - 1 DAY
            + INTERVAL SoNgayNghi - 1 DAY;

        IF NOT EXISTS (SELECT * FROM holidays) THEN
        BEGIN
/* region: Process late shift employees situation */
            SET gioTanLamHomNay =
            (SELECT ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY,
                (CASE DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY)
                    WHEN 1 THEN GioTanLamChuNhat
                    WHEN 7 THEN GioTanLamThuBay
                    ELSE GioTanLamCacNgayTrongTuan
                END))
            FROM thoigianbieutuan
            WHERE DATE(ThoiDiemTao) <= CURRENT_DATE() - INTERVAL 1 DAY
            ORDER BY ThoiDiemTao DESC
            LIMIT 1);

            CREATE TEMPORARY TABLE working_employees
            SELECT *
            FROM chamcong c
            WHERE DATE(ThoiGianVaoLam) = CURRENT_DATE() - INTERVAL 1 DAY
            AND ThoiGianTanLam IS NULL;

            UPDATE sogiolamtrongngay
            SET SoGioLamTrongGio =
                IF (gioTanLamHomNay > (SELECT ThoiGianVaoLam
                                FROM working_employees we
                                WHERE we.IDNhanVien = IDNhanVien)
                    , SoGioLamTrongGio +
                    TIMESTAMPDIFF (
                        HOUR,
                        gioTanLamHomNay,
                        (SELECT ThoiGianVaoLam
                        FROM working_employees we
                        WHERE we.IDNhanVien = IDNhanVien)
                        )
                    , SoGioLamTrongGio)
                ,
                SoGioLamNgoaiGio =
                    IF (gioTanLamHomNay > (SELECT ThoiGianVaoLam
                                    FROM working_employees we
                                    WHERE we.IDNhanVien = IDNhanVien)
                        , SoGioLamNgoaiGio +
                          TIMESTAMPDIFF(
                              HOUR,
                              NOW(),
                              gioTanLamHomNay
                              )
                        , TIMESTAMPDIFF(
                            HOUR,
                            NOW(),
                            (SELECT ThoiGianVaoLam
                            FROM working_employees we
                            WHERE we.IDNhanVien = IDNhanVien)
                            ))
            WHERE Ngay = CURRENT_DATE() - 1
            AND IDNhanVien IN (SELECT IDNhanVien FROM working_employees);

            UPDATE chamcong
            SET ThoiGianTanLam = NOW()
            WHERE IDNhanVien in (SELECT IDNhanVien FROM working_employees);

            INSERT INTO chamcong (ThoiGianVaoLam, IDNhanVien,  ThoiGianTanLam)
            SELECT NOW(), IDNhanVien, NULL
            FROM working_employees;

            DROP TEMPORARY TABLE working_employees;

/* endregion */

            INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
            SELECT CURRENT_DATE(), ID, 0, 0
            FROM nhanvien;
        END;
        END IF;

/* region: Check and calculate payment for employees */

        /* calc payment for employees with CachTinhLuong TheoThang */
        IF (CURRENT_DATE() = (
            SELECT NgayTinhLuongHangThang FROM thamso WHERE DATE(ThoiDiemTao) <= CURRENT_DATE()
        )) THEN
        BEGIN
            INSERT INTO luong (NgayTinhLuong, IDNhanVien, TienLuong, TienTruLuong, TienThuong, TongTienLuong)
            SELECT DISTINCT
                   CURRENT_DATE(),
                   n.ID,
                   c.TienLuongMoiThang,
                   (
                    SELECT SUM(SoTienTru + SoPhanTramTru * c.TienLuongMoiThang)
                    FROM truluong t
                    WHERE (Ngay BETWEEN CURRENT_DATE()
                        AND (SELECT NgayTinhLuongHangThang FROM thamso WHERE DATE(ThoiDiemTao) <= CURRENT_DATE()))
                    AND t.IDNhanVien = n.ID
                   ),
                   (
                    SELECT c.TienLuongMoiThang / 26 / (
                    SELECT HOUR(TIMEDIFF(GioTanLamCacNgayTrongTuan, GioVaoLamCacNgayTrongTuan))
                    FROM thoigianbieutuan
                    WHERE DATE(ThoiDiemTao) <= CURRENT_DATE() - 1
                    ) * s.SoGioLamNgoaiGio * c.PhanTramLuongNgoaiGio
                   ),
                   c.TienLuongMoiThang
            FROM nhanvien n
            INNER JOIN chucvu c ON n.ChucVu = c.TenChucVu
            INNER JOIN sogiolamtrongngay s on s.IDNhanVien = n.ID
            WHERE c.CachTinhLuong = 'TheoThang';

            UPDATE luong l
            SET /*TienTruLuong = (
                SELECT SUM(SoTienTru + SoPhanTramTru * l.TienLuong)
                FROM truluong t
                WHERE (Ngay BETWEEN CURRENT_DATE()
                    AND (SELECT NgayTinhLuongHangThang FROM thamso WHERE DATE(ThoiDiemTao) <= CURRENT_DATE()))
                AND t.IDNhanVien = l.IDNhanVien
            ),
            TienThuong = (
                SELECT l.TienLuong / 26 / (
                    SELECT HOUR(TIMEDIFF(GioTanLamCacNgayTrongTuan, GioVaoLamCacNgayTrongTuan))
                    FROM thoigianbieutuan
                    WHERE DATE(ThoiDiemTao) <= CURRENT_DATE() - 1
                ) * s.SoGioLamNgoaiGio * c.PhanTramLuongNgoaiGio
                FROM chucvu c
                INNER JOIN nhanvien n on c.TenChucVu = n.ChucVu
                INNER JOIN sogiolamtrongngay s on s.IDNhanVien = n.ID
                WHERE n.ID = l.IDNhanVien
                AND (s.Ngay BETWEEN CURRENT_DATE() AND
                    (SELECT LanTraLuongCuoi FROM cachtinhluong ctl WHERE ctl.Ten = 'TheoThang'))
            ),*/
            TongTienLuong = TongTienLuong + TienThuong - TienTruLuong
            WHERE NgayTinhLuong = CURRENT_DATE()
            AND IDNhanVien IN (
                SELECT ID
                FROM nhanvien
                WHERE ChucVu IN (SELECT TenChucVu FROM chucvu WHERE CachTinhLuong = 'TheoThang')
            );

            UPDATE cachtinhluong
            SET LanTraLuongCuoi = CURRENT_DATE()
            WHERE Ten = 'TheoThang';
        END;
        END IF;

        /* calc payment for employees with CachTinhLuong 'TheoGio' */
        IF (CURRENT_DATE() =
            (SELECT LanTraLuongCuoi + INTERVAL KyHanTraLuongTheoNgay DAY
            FROM cachtinhluong WHERE Ten = 'TheoGio')) THEN
        BEGIN
            INSERT INTO luong (NgayTinhLuong, IDNhanVien, TienLuong, TienTruLuong, TienThuong, TongTienLuong)
            SELECT DISTINCT
                   CURRENT_DATE(),
                   n.ID,
                   c.TienLuongMoiGio * s.SoGioLamTrongGio,
                   (
                    SELECT SUM(SoTienTru + SoPhanTramTru * c.TienLuongMoiThang)
                    FROM truluong t
                    WHERE (Ngay BETWEEN CURRENT_DATE()
                        AND (SELECT NgayTinhLuongHangThang FROM thamso WHERE DATE(ThoiDiemTao) <= CURRENT_DATE()))
                    AND t.IDNhanVien = n.ID
                   ),
                   c.TienLuongMoiGio * c.PhanTramLuongNgoaiGio * s.SoGioLamNgoaiGio,
                   c.TienLuongMoiGio * s.SoGioLamTrongGio
            FROM nhanvien n
            INNER JOIN chucvu c ON n.ChucVu = c.TenChucVu
            INNER JOIN sogiolamtrongngay s ON n.ID = s.IDNhanVien
            WHERE c.CachTinhLuong;

            UPDATE luong
            SET TongTienLuong = TongTienLuong + TienThuong - TienTruLuong
            WHERE NgayTinhLuong = CURRENT_DATE()
            AND IDNhanVien IN (
                SELECT ID
                FROM nhanvien
                WHERE ChucVu IN (SELECT TenChucVu FROM chucvu WHERE CachTinhLuong = 'TheoGio')
            );

            UPDATE cachtinhluong
            SET LanTraLuongCuoi = CURRENT_DATE()
            WHERE Ten = 'TheoGio';
        END;
        END IF;

/* endregion */

    END $


DROP PROCEDURE IF EXISTS UpdateStartBusinessHoursEvent;
CREATE PROCEDURE UpdateStartBusinessHoursEvent()
BEGIN
    DECLARE businessHour datetime;
    SET businessHour = (
        SELECT
        (ADDTIME(CURRENT_DATE() + INTERVAL 1 DAY,
            (CASE DAYOFWEEK(CURRENT_DATE() + INTERVAL 1 DAY)
                WHEN 1 THEN GioVaoLamChuNhat
                WHEN 7 THEN GioVaoLamThuBay
                ELSE GioVaoLamCacNgayTrongTuan
            END)))
        FROM thoigianbieutuan
        WHERE DATE(ThoiDiemTao) <= CURRENT_DATE()
        ORDER BY ThoiDiemTao DESC
        LIMIT 1
    );

    ALTER EVENT StartBusinessHoursEvent
    ON SCHEDULE AT businessHour;
END $


DROP EVENT IF EXISTS DailyUpdateStartBusinessHoursEvent;
CREATE EVENT DailyUpdateStartBusinessHoursEvent
ON SCHEDULE EVERY 1 DAY
STARTS (TIMESTAMP(CURRENT_DATE()) + INTERVAL 23 HOUR + INTERVAL 59 MINUTE)
DO
    BEGIN
        CALL UpdateStartBusinessHoursEvent;
    END $

DELIMITER ;