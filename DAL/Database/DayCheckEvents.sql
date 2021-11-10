DELIMITER $

DROP EVENT IF EXISTS START_BUSINESS_HOURS_EVENT;
CREATE EVENT START_BUSINESS_HOURS_EVENT
ON SCHEDULE AT (
    CURRENT_DATE() + INTERVAL 1 DAY + INTERVAL 6 HOUR /*+ INTERVAL 12 HOUR + INTERVAL 18 MINUTE*/
    )
ON COMPLETION PRESERVE
DO
    BEGIN
        DECLARE homQua date;
        DECLARE gioTanLamHomQua datetime;
        DECLARE gioVaoLamHomQua datetime;

        SET homQua = CURRENT_DATE() - INTERVAL 1 DAY;

        SET gioVaoLamHomQua =
            (SELECT ADDTIME(homQua,
                (CASE DAYOFWEEK(homQua)
                    WHEN 1 THEN GioVaoLamChuNhat
                    WHEN 7 THEN GioVaoLamThuBay
                    ELSE GioVaoLamCacNgayTrongTuan
                END))
            FROM thoigianbieutuan
            WHERE DATE(ThoiDiemTao) <= homQua
            ORDER BY ThoiDiemTao DESC
            LIMIT 1);

        SET gioTanLamHomQua =
            (SELECT ADDTIME(homQua,
                (CASE DAYOFWEEK(homQua)
                    WHEN 1 THEN GioTanLamChuNhat
                    WHEN 7 THEN GioTanLamThuBay
                    ELSE GioTanLamCacNgayTrongTuan
                END))
            FROM thoigianbieutuan
            WHERE DATE(ThoiDiemTao) <= homQua
            ORDER BY ThoiDiemTao DESC
            LIMIT 1);

/* region: Process late shift employees situation */

        CREATE TEMPORARY TABLE working_employees
        SELECT *
        FROM chamcong c
        WHERE DATE(ThoiGianVaoLam) = homQua
        AND ThoiGianTanLam IS NULL;

        CREATE TEMPORARY TABLE we2
        SELECT * FROM working_employees;

        CREATE TEMPORARY TABLE we3
        SELECT * FROM working_employees;

        CREATE TEMPORARY TABLE we4
        SELECT * FROM working_employees;

        CREATE TEMPORARY TABLE we5
        SELECT * FROM working_employees;

        INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
        SELECT homQua, ID, 0, 0
        FROM nhanvien
        WHERE NgayVaoLam <= homQua
        AND NgayThoiViec IS NULL;

        UPDATE sogiolamtrongngay s
        SET SoGioLamTrongGio =
            IF (gioTanLamHomQua > (SELECT ThoiGianVaoLam
                            FROM working_employees
                            WHERE working_employees.IDNhanVien = s.IDNhanVien)
                , SoGioLamTrongGio +
                TIMESTAMPDIFF (
                    HOUR,
                    (SELECT ThoiGianVaoLam
                    FROM we2
                    WHERE we2.IDNhanVien = s.IDNhanVien),
                    gioTanLamHomQua
                    )
                , SoGioLamTrongGio)
            ,
            SoGioLamNgoaiGio =
                IF (gioTanLamHomQua > (SELECT ThoiGianVaoLam
                                FROM we3
                                WHERE we3.IDNhanVien = s.IDNhanVien)
                    , SoGioLamNgoaiGio +
                      TIMESTAMPDIFF(
                          HOUR,
                          gioTanLamHomQua,
                          NOW()
                          )
                    , TIMESTAMPDIFF(
                        HOUR,
                        (SELECT ThoiGianVaoLam
                        FROM we4
                        WHERE we4.IDNhanVien = s.IDNhanVien),
                        NOW()
                        ))
        WHERE Ngay = homQua
        AND s.IDNhanVien IN (SELECT we5.IDNhanVien FROM we5);

        UPDATE chamcong
        SET ThoiGianTanLam = NOW()
        WHERE IDNhanVien in (SELECT IDNhanVien FROM working_employees);

        INSERT INTO chamcong (ThoiGianVaoLam, IDNhanVien, ThoiGianTanLam)
        SELECT NOW(), IDNhanVien, NULL
        FROM working_employees;

        DROP TEMPORARY TABLE working_employees;
        DROP TEMPORARY TABLE we2;
        DROP TEMPORARY TABLE we3;
        DROP TEMPORARY TABLE we4;
        DROP TEMPORARY TABLE we5;
/* endregion */


/* region: Update truluong due to DiTre or VangMat */

        CREATE TEMPORARY TABLE nhanvien_hientai
        SELECT *
        FROM nhanvien
        WHERE NgayVaoLam <= homQua
        AND NgayThoiViec IS NULL;

        CREATE TEMPORARY TABLE nv_ht2
        SELECT * FROM nhanvien_hientai;

        CREATE TEMPORARY TABLE nv_ht3
        SELECT * FROM nhanvien_hientai;

        CREATE TEMPORARY TABLE nv_ht4
        SELECT * FROM nhanvien_hientai;

        CREATE TEMPORARY TABLE nv_ht5
        SELECT * FROM nhanvien_hientai;

        CREATE TEMPORARY TABLE timeVariables
        SELECT ThoiGianChoPhepDiTre, ThoiGianChoPhepVeSom,
               ThoiGianDiTreToiDa, ThoiGianVeSomToiDa
        FROM thamso
        WHERE DATE(ThoiDiemTao) <= homQua - INTERVAL 1 DAY
        ORDER BY ThoiDiemTao
        LIMIT 1;

        CREATE TEMPORARY TABLE time2
        SELECT * FROM timeVariables;

        CREATE TEMPORARY TABLE time3
        SELECT * FROM timeVariables;

        /* Them nhan vien vang mat vao bang NghiPhep */
        INSERT INTO nghiphep (IDNhanVien, NgayBatDauNghi, SoNgayNghi, LyDoNghi, CoPhep)
        SELECT IDNhanVien,
               homQua,
               1,
               'Tu dong them nhan vien vang mat',
               0
        FROM chamcong
        WHERE (
            SELECT ThoiGianVaoLam
            FROM chamcong c
            WHERE DATE(c.ThoiGianVaoLam) = homQua
            GROUP BY c.IDNhanVien, DATE(c.ThoiGianVaoLam)
            ORDER BY c.ThoiGianVaoLam ASC
            LIMIT 1
        ) > ADDTIME(gioVaoLamHomQua, (SELECT ThoiGianDiTreToiDa FROM timeVariables))
        UNION
        SELECT IDNhanVien,
               homQua,
               1,
               'Tu dong them nhan vien vang mat',
               0
        FROM nhanvien_hientai n
        WHERE n.ID NOT IN (SELECT IDNhanVien FROM chamcong c2 WHERE DATE(c2.ThoiGianVaoLam) = homQua);

        /* Tru luong cac nhan vien vang mat hoac di tre */
        INSERT INTO truluong (Ngay, IDNhanVien, TenViPham, SoTienTru, SoPhanTramTru, GhiChu)
    /* Nhan vien vang mat */
        SELECT homQua,
               IDNhanVien,
               'VangMat',
               (SELECT TruLuongTrucTiep FROM cacloaivipham WHERE TenViPham = 'VangMat'),
               (SELECT TruLuongTheoPhanTram FROM cacloaivipham WHERE TenViPham = 'VangMat'),
               'Tu dong tru luong nhan vien vang mat'
        FROM nghiphep
        WHERE NgayBatDauNghi = homQua AND CoPhep = 0
        UNION
    /* Nhan vien di tre */
        SELECT homQua,
               IDNhanVien,
               'DiTre',
               (SELECT TruLuongTrucTiep FROM cacloaivipham WHERE TenViPham = 'DiTre'),
               (SELECT TruLuongTheoPhanTram FROM cacloaivipham WHERE TenViPham = 'DiTre'),
               'Tu dong tru luong nhan vien di tre'
        FROM chamcong
        WHERE (
            SELECT ThoiGianVaoLam
            FROM chamcong c
            WHERE DATE(c.ThoiGianVaoLam) = homQua
            GROUP BY c.IDNhanVien, DATE(c.ThoiGianVaoLam)
            ORDER BY c.ThoiGianVaoLam ASC
            LIMIT 1
        ) BETWEEN ADDTIME(gioVaoLamHomQua, (SELECT ThoiGianChoPhepDiTre FROM time2)) + INTERVAL 1 SECOND
            AND (SELECT ThoiGianDiTreToiDa FROM time3);
/* endregion */


/* region: Check and calculate payment for employees */

        /* calc payment for employees with CachTinhLuong TheoThang */
        IF (CURRENT_DATE() = (
            SELECT NgayTinhLuongThangNay from cachtinhluong WHERE Ten = 'TheoThang'
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
                    WHERE (Ngay BETWEEN c2.LanTraLuongCuoi AND homQua)
                    AND t.IDNhanVien = n.ID
                   ),
                   (
                   SELECT SUM(s.SoGioLamNgoaiGio) * c.PhanTramLuongNgoaiGio * c.TienLuongMoiThang / 26 /
                    (
                        SELECT HOUR(TIMEDIFF(GioTanLamCacNgayTrongTuan, GioVaoLamCacNgayTrongTuan))
                        FROM thoigianbieutuan
                        WHERE DATE(ThoiDiemTao) <= homQua
                    )
                   ),
                   c.TienLuongMoiThang
            FROM nv_ht2 n
            INNER JOIN chucvu c ON n.ChucVu = c.TenChucVu
            INNER JOIN sogiolamtrongngay s on s.IDNhanVien = n.ID
            INNER JOIN cachtinhluong c2 on c.CachTinhLuong = c2.Ten
            WHERE c.CachTinhLuong = 'TheoThang'
            AND (s.Ngay BETWEEN c2.LanTraLuongCuoi AND homQua)
            GROUP BY n.ID;

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
                FROM nv_ht3
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
                   c.TienLuongMoiGio * (SELECT SUM(s.SoGioLamTrongGio)),
                   (
                    SELECT SUM(SoTienTru)
                    FROM truluong t
                    WHERE t.Ngay BETWEEN c3.LanTraLuongCuoi AND homQua
                    AND t.IDNhanVien = n.ID
                   ),
                   c.TienLuongMoiGio * c.PhanTramLuongNgoaiGio * (SELECT SUM(s.SoGioLamNgoaiGio)),
                   c.TienLuongMoiGio * (SELECT SUM(s.SoGioLamTrongGio))
            FROM nv_ht4 n
            INNER JOIN chucvu c ON n.ChucVu = c.TenChucVu
            INNER JOIN sogiolamtrongngay s ON n.ID = s.IDNhanVien
            INNER JOIN cachtinhluong c3 on c.CachTinhLuong = c3.Ten
            WHERE c.CachTinhLuong = 'TheoGio'
            AND (s.Ngay BETWEEN c3.LanTraLuongCuoi AND homQua)
            GROUP BY n.ID;

            UPDATE luong l
            SET TienTruLuong = TienTruLuong - GREATEST((
                    SELECT SUM(SoPhanTramTru)
                    FROM truluong t
                    WHERE t.Ngay BETWEEN (SELECT LanTraLuongCuoi FROM cachtinhluong WHERE Ten = 'TheoGio')
                        AND homQua
                    AND t.IDNhanVien = l.IDNhanVien
                ), 100) / 100 * l.TienLuong,
                TongTienLuong = TongTienLuong + TienThuong - TienTruLuong
            WHERE NgayTinhLuong = CURRENT_DATE()
            AND IDNhanVien IN (
                SELECT ID
                FROM nv_ht5
                WHERE ChucVu IN (SELECT TenChucVu FROM chucvu WHERE CachTinhLuong = 'TheoGio')
            );

            UPDATE cachtinhluong
            SET LanTraLuongCuoi = CURRENT_DATE()
            WHERE Ten = 'TheoGio';
        END;
        END IF;

/* endregion */

    END $


DROP PROCEDURE IF EXISTS UPDATE_START_BUSINESS_HOURS;
CREATE PROCEDURE UPDATE_START_BUSINESS_HOURS()
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
        WHERE ThoiDiemTao <= NOW()
        ORDER BY ThoiDiemTao DESC
        LIMIT 1
    );

    ALTER EVENT START_BUSINESS_HOURS_EVENT
    ON SCHEDULE AT businessHour
    ENABLE;
END $


DROP EVENT IF EXISTS DAILY_UPDATE_START_BUSINESS_HOURS_EVENT;
CREATE EVENT DAILY_UPDATE_START_BUSINESS_HOURS_EVENT
ON SCHEDULE EVERY 1 DAY
STARTS (TIMESTAMP(CURRENT_DATE()) + INTERVAL 23 HOUR + INTERVAL 59 MINUTE /*+ INTERVAL 47 MINUTE*/)
DO
    BEGIN
        DECLARE ngayMai date;
        DECLARE coLamNgayMai tinyint;

        SET ngayMai = CURRENT_DATE() + INTERVAL 1 DAY;
        SET coLamNgayMai = (
            SELECT (
                CASE DAYOFWEEK(ngayMai)
                    WHEN 2 THEN CoLamThuHai
                    WHEN 3 THEN CoLamThuBa
                    WHEN 4 THEN CoLamThuTu
                    WHEN 5 THEN CoLamThuNam
                    WHEN 6 THEN CoLamThuSau
                    WHEN 7 THEN CoLamThuBay
                    WHEN 1 THEN CoLamChuNhat
                END
               )
            FROM thamso
            WHERE ThoiDiemTao <= NOW()
            );

        /* check if business is currently having a holiday */
        CREATE TEMPORARY TABLE incomingHolidays
        SELECT *
        FROM nghile
        WHERE ngayMai BETWEEN MAKEDATE(YEAR(ngayMai), 1)
            + INTERVAL Thang - 1 MONTH
            + INTERVAL Ngay - 1 DAY
        AND MAKEDATE(YEAR(ngayMai), 1)
            + INTERVAL Thang - 1 MONTH
            + INTERVAL Ngay - 1 DAY
            + INTERVAL SoNgayNghi - 1 DAY;

        IF NOT EXISTS(SELECT * FROM incomingHolidays) AND coLamNgayMai = 1 THEN
            CALL UPDATE_START_BUSINESS_HOURS;
        END IF;
    END $

DELIMITER ;