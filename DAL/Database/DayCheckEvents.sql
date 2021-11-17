DELIMITER $

DROP EVENT IF EXISTS START_BUSINESS_HOURS_EVENT;
CREATE EVENT START_BUSINESS_HOURS_EVENT
ON SCHEDULE
    EVERY 1 DAY
    STARTS CURRENT_DATE() + INTERVAL 1 DAY + INTERVAL 6 HOUR
ON COMPLETION PRESERVE
DO
    BEGIN
        DECLARE homQua date;
        DECLARE gioTanLamHomQua datetime;
        DECLARE gioVaoLamHomQua datetime;

        DELETE FROM eventlog;

        SET homQua = CURRENT_DATE() - INTERVAL 1 DAY;

        SET gioVaoLamHomQua =
            (SELECT ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY,
                (CASE DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY)
                    WHEN 1 THEN GioVaoLamChuNhat
                    WHEN 7 THEN GioVaoLamThuBay
                    ELSE GioVaoLamCacNgayTrongTuan
                END))
            FROM thoigianbieutuan
            WHERE DATE(ThoiDiemTao) <= CURRENT_DATE() - INTERVAL 1 DAY
            ORDER BY ThoiDiemTao DESC
            LIMIT 1);

        SET gioTanLamHomQua =
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

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'gia tri cac bien thoi gian:', homQua, GioVaoLamHomQua, GioTanLamHomQua);

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

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'tao bang temp working_employees hoan tat', null, null, null);

        INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
        SELECT homQua, ID, 0.0, 0.0
        FROM nhanvien
        WHERE NgayVaoLam <= homQua
        AND NgayThoiViec IS NULL;

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'insert cac dong moi vao sogiolamtrongngay hoan tat', null, null, null);

        UPDATE sogiolamtrongngay s
        SET SoGioLamTrongGio =
            IF (gioTanLamHomQua > (SELECT ThoiGianVaoLam
                            FROM working_employees
                            WHERE working_employees.IDNhanVien = s.IDNhanVien)
                , SoGioLamTrongGio +
                TIMESTAMPDIFF (
                    MINUTE,
                    (SELECT ThoiGianVaoLam
                    FROM we2
                    WHERE we2.IDNhanVien = s.IDNhanVien),
                    gioTanLamHomQua
                    ) / 60
                , SoGioLamTrongGio)
            ,
            SoGioLamNgoaiGio =
                IF (gioTanLamHomQua > (SELECT ThoiGianVaoLam
                                FROM we3
                                WHERE we3.IDNhanVien = s.IDNhanVien)
                    , SoGioLamNgoaiGio +
                      TIMESTAMPDIFF(
                          MINUTE,
                          gioTanLamHomQua,
                          NOW()
                          ) / 60
                    , TIMESTAMPDIFF(
                        MINUTE,
                        (SELECT ThoiGianVaoLam
                        FROM we4
                        WHERE we4.IDNhanVien = s.IDNhanVien),
                        NOW()
                        ) / 60)
        WHERE Ngay = homQua
        AND s.IDNhanVien IN (SELECT we5.IDNhanVien FROM we5);

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'Update sogiolamtrongngay hoan tat', NULL, NULL, NULL);

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

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'Update table chamcong hoan tat', NULL, NULL, NULL);
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

        CREATE TEMPORARY TABLE nv_ht6
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

        CREATE TEMPORARY TABLE time4
        SELECT * FROM timeVariables;

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'tao bang temp nhanvien_hientai va timeVariables hoan tat', NULL, NULL, NULL);

        /* Them nhan vien vang mat vao bang NghiPhep */
        INSERT INTO nghiphep (IDNhanVien, NgayBatDauNghi, SoNgayNghi, LyDoNghi, CoPhep)
        /* Nhan vien bo ve som qua thoi gian ve som toi da */
        SELECT IDNhanVien,
               homQua,
               1,
               'Tu dong them nhan vien bo ve som qua thoi gian',
               0
        FROM chamcong ch
        WHERE (
            SELECT c.ThoiGianTanLam
            FROM chamcong c
            WHERE DATE(c.ThoiGianVaoLam) = homQua
            AND c.IDNhanVien = ch.IDNhanVien
            ORDER BY c.ThoiGianTanLam DESC
            LIMIT 1
        ) < SUBTIME(gioTanLamHomQua, (SELECT ThoiGianVeSomToiDa FROM time4))
        AND ch.IDNhanVien NOT IN (
            SELECT IDNhanVien
            FROM nghiphep np
            WHERE homQua BETWEEN np.NgayBatDauNghi AND np.NgayBatDauNghi + INTERVAL np.SoNgayNghi - 1 DAY
        )
        UNION
        /* Nhan vien khong di lam */
        SELECT n.ID as IDNhanVien,
               homQua,
               1,
               'Tu dong them nhan vien vang mat',
               0
        FROM nhanvien_hientai n
        WHERE n.ID NOT IN (
            SELECT IDNhanVien
            FROM chamcong c
            WHERE DATE(c.ThoiGianVaoLam) = homQua
            UNION
            SELECT IDNhanVien
            FROM nghiphep np
            WHERE homQua BETWEEN np.NgayBatDauNghi AND np.NgayBatDauNghi + INTERVAL np.SoNgayNghi - 1 DAY
        );

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'Them nhan vien vang mat vao bang NghiPhep hoan tat', NULL, NULL, NULL);

        /* Tru luong cac nhan vien vang mat va cac nhan vien bo ve som */
        INSERT INTO truluong (Ngay, IDNhanVien, TenViPham, SoTienTru, SoPhanTramTru, GhiChu)
        SELECT homQua,
               IDNhanVien,
               'VangMat',
               (SELECT TruLuongTrucTiep FROM cacloaivipham WHERE TenViPham = 'VangMat'),
               (SELECT TruLuongTheoPhanTram FROM cacloaivipham WHERE TenViPham = 'VangMat'),
               'Tu dong tru luong nhan vien vang mat'
        FROM nghiphep
        WHERE NgayBatDauNghi = homQua AND CoPhep = 0
        UNION
        SELECT homQua,
               IDNhanVien,
               'VeSom',
               (SELECT TruLuongTrucTiep FROM cacloaivipham WHERE TenViPham = 'VeSom'),
               (SELECT TruLuongTheoPhanTram FROM cacloaivipham WHERE TenViPham = 'VeSom'),
               'Tu dong tru luong nhan vien bo ve som'
        FROM chamcong ch
        WHERE ThoiGianTanLam BETWEEN SUBTIME(gioTanLamHomQua, (SELECT ThoiGianVeSomToiDa FROM time2))
            AND SUBTIME(gioTanLamHomQua, (SELECT ThoiGianChoPhepVeSom FROM time3) + INTERVAL 1 SECOND)
        AND ThoiGianTanLam = (
            SELECT ThoiGianTanLam
            FROM chamcong c
            WHERE c.IDNhanVien = ch.IDNhanVien
            AND DATE(ThoiGianVaoLam) = homQua
            ORDER BY ThoiGianTanLam DESC
            LIMIT 1
        );

        DROP TEMPORARY TABLE time2;
        DROP TEMPORARY TABLE time3;
        DROP TEMPORARY TABLE time4;

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'Tru luong nhan vien vang mat hoac bo ve som hoan tat', NULL, NULL, NULL);
/* endregion */


/* region: Check and calculate payment for employees */

        /* calc payment for employees with CachTinhLuong TheoThang */
        IF (CURRENT_DATE() = (
            SELECT NgayTinhLuongThangNay from cachtinhluong WHERE Ten = 'TheoThang'
        )) THEN
        BEGIN
            /* !!!!LOG!!!! */
            INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
            VALUES (NOW(), 'Hom nay la ngay tra luong cho nhan vien theo thang', NULL, NULL, NULL);

            INSERT INTO luong (NgayTinhLuong, IDNhanVien, TienLuong, TienTruLuong, TienThuong, TongTienLuong)
            SELECT DISTINCT
                   CURRENT_DATE(),
                   n.ID,
                   c.TienLuongMoiThang,
                   IFNULL((
                    SELECT SUM(SoTienTru + SoPhanTramTru / 100.0 * c.TienLuongMoiThang)
                    FROM truluong t
                    WHERE (Ngay BETWEEN c2.LanTraLuongCuoi AND homQua)
                    AND t.IDNhanVien = n.ID
                   ), 0),
                   IFNULL((
                   SELECT SUM(s.SoGioLamNgoaiGio) * c.PhanTramLuongNgoaiGio * c.TienLuongMoiThang / 26 /
                    (
                        SELECT HOUR(TIMEDIFF(GioTanLamCacNgayTrongTuan, GioVaoLamCacNgayTrongTuan))
                        FROM thoigianbieutuan
                        WHERE DATE(ThoiDiemTao) <= homQua
                    )
                   ), 0),
                   c.TienLuongMoiThang
            FROM nv_ht2 n
            INNER JOIN chucvu c ON n.ChucVu = c.TenChucVu
            INNER JOIN sogiolamtrongngay s on s.IDNhanVien = n.ID
            INNER JOIN cachtinhluong c2 on c.CachTinhLuong = c2.Ten
            WHERE c.CachTinhLuong = 'TheoThang'
            AND (s.Ngay BETWEEN c2.LanTraLuongCuoi AND homQua)
            GROUP BY n.ID;

            /* !!!!LOG!!!! */
            INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
            VALUES (NOW(), 'LUONG THANG: insert cac dong moi vao table luong hoan tat', NULL, NULL, NULL);

            UPDATE luong l
            SET TongTienLuong = GREATEST(TongTienLuong + TienThuong - TienTruLuong, 0)
            WHERE NgayTinhLuong = CURRENT_DATE()
            AND IDNhanVien IN (
                SELECT ID
                FROM nv_ht3
                WHERE ChucVu IN (SELECT TenChucVu FROM chucvu WHERE CachTinhLuong = 'TheoThang')
            );

            /* !!!!LOG!!!! */
            INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
            VALUES (NOW(), 'LUONG THANG: Update tong tien luong trong table luong hoan tat', NULL, NULL, NULL);

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
            /* !!!!LOG!!!! */
            INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
            VALUES (NOW(), 'Hom nay la ngay tra luong cho nhan vien theo gio', NULL, NULL, NULL);

            INSERT INTO luong (NgayTinhLuong, IDNhanVien, TienLuong, TienTruLuong, TienThuong, TongTienLuong)
            SELECT DISTINCT
                   CURRENT_DATE(),
                   n.ID,
                   c.TienLuongMoiGio * (SELECT SUM(s.SoGioLamTrongGio)),
                   IFNULL((
                    SELECT SUM(SoTienTru)
                    FROM truluong t
                    WHERE t.Ngay BETWEEN c3.LanTraLuongCuoi AND homQua
                    AND t.IDNhanVien = n.ID
                   ), 0),
                   c.TienLuongMoiGio * c.PhanTramLuongNgoaiGio * (SELECT SUM(s.SoGioLamNgoaiGio)),
                   c.TienLuongMoiGio * (SELECT SUM(s.SoGioLamTrongGio))
            FROM nv_ht4 n
            INNER JOIN chucvu c ON n.ChucVu = c.TenChucVu
            INNER JOIN sogiolamtrongngay s ON n.ID = s.IDNhanVien
            INNER JOIN cachtinhluong c3 on c.CachTinhLuong = c3.Ten
            WHERE c.CachTinhLuong = 'TheoGio'
            AND (s.Ngay BETWEEN c3.LanTraLuongCuoi AND homQua)
            GROUP BY n.ID;

            /* !!!!LOG!!!! */
            INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
            VALUES (NOW(), 'LUONG NGAY: insert cac dong moi vao table luong hoan tat', NULL, NULL, NULL);

            UPDATE luong l
            SET TienTruLuong = TienTruLuong + LEAST(IFNULL((
                    SELECT SUM(SoPhanTramTru)
                    FROM truluong t
                    WHERE t.Ngay BETWEEN (SELECT LanTraLuongCuoi FROM cachtinhluong WHERE Ten = 'TheoGio')
                        AND homQua
                    AND t.IDNhanVien = l.IDNhanVien
                ), 0.0), 100.0) / 100.0 * l.TienLuong
            WHERE NgayTinhLuong = CURRENT_DATE()
            AND IDNhanVien IN (
                SELECT ID
                FROM nv_ht5
                WHERE ChucVu IN (SELECT TenChucVu FROM chucvu WHERE CachTinhLuong = 'TheoGio')
            );

            UPDATE luong l
            SET TongTienLuong = GREATEST(TongTienLuong + TienThuong - TienTruLuong, 0)
            WHERE NgayTinhLuong = CURRENT_DATE()
            AND IDNhanVien IN (
                SELECT ID
                FROM nv_ht6
                WHERE ChucVu IN (SELECT TenChucVu FROM chucvu WHERE CachTinhLuong = 'TheoGio')
            );

            /* !!!!LOG!!!! */
            INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
            VALUES (NOW(), 'LUONG NGAY: Update tong tien luong trong table luong hoan tat', NULL, NULL, NULL);

            UPDATE cachtinhluong
            SET LanTraLuongCuoi = CURRENT_DATE()
            WHERE Ten = 'TheoGio';
        END;
        END IF;

        /* Them dong va cap nhat baocaochamcong va baocaonhansu */

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'Bat dau kiem tra va insert cac dong moi vao cac table bao cao', NULL, NULL, NULL);

        IF (DAY(CURRENT_DATE()) = 1) THEN
            BEGIN
                /* !!!!LOG!!!! */
                INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
                VALUES (NOW(), 'Hom nay la ngay dau cua thang, insert baocaonhansu moi', NULL, NULL, NULL);

                INSERT INTO baocaonhansu (Thang, Nam, SoNhanVienMoi, SoNhanVienThoiViec)
                VALUES (MONTH(CURRENT_DATE()), YEAR(CURRENT_DATE()), 0, 0);
            END;
        END IF;

        IF (
            SELECT (
                CASE DAYOFWEEK(CURRENT_DATE())
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
            ) THEN
        BEGIN
            /* !!!!LOG!!!! */
            INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
            VALUES (NOW(), 'Hom nay co lam, bat dau insert bao cao moi', NULL, NULL, NULL);

            INSERT INTO baocaochamcong (NgayBaoCao,
                                       SoNVDenSom,
                                       SoNVDenDungGio,
                                       SoNVDenTre,
                                       SoNVTanLamSom,
                                       SoNVTanLamDungGio,
                                       SoNVLamThemGio)
            VALUES (CURRENT_DATE(), 0, 0, 0, 0, 0, 0);
        END;
        END IF;

        /* !!!!LOG!!!! */
            INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
            VALUES (NOW(), 'Insert cac bao cao moi hoan tat', NULL, NULL, NULL);

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'Insert baocaochamcong va baocaonhansu bat dau cho ngay hien tai hoan tat', NULL, NULL, NULL);

        CREATE TEMPORARY TABLE time5
        SELECT * FROM timeVariables;

        CREATE TEMPORARY TABLE time6
        SELECT * FROM timeVariables;

        UPDATE baocaochamcong
        SET SoNVTanLamSom = (
            SELECT COUNT(*)
            FROM chamcong ch
            WHERE ThoiGianTanLam < SUBTIME(gioTanLamHomQua, (SELECT ThoiGianChoPhepVeSom from time6))
            AND ThoiGianTanLam = (
                SELECT ThoiGianTanLam
                FROM chamcong c
                WHERE c.IDNhanVien = ch.IDNhanVien
                AND DATE(ThoiGianVaoLam) = homQua
                ORDER BY ThoiGianTanLam DESC
                LIMIT 1
            )
        ), SoNVTanLamDungGio = (
            SELECT COUNT(*)
            FROM chamcong ch
            WHERE ThoiGianTanLam BETWEEN SUBTIME(gioTanLamHomQua, (SELECT ThoiGianChoPhepVeSom from time5))
                AND gioTanLamHomQua + INTERVAL 1 HOUR - INTERVAL 1 SECOND
            AND ThoiGianTanLam = (
                SELECT ThoiGianTanLam
                FROM chamcong c
                WHERE c.IDNhanVien = ch.IDNhanVien
                AND DATE(ThoiGianVaoLam) = homQua
                ORDER BY ThoiGianTanLam DESC
                LIMIT 1
            )
        ), SoNVLamThemGio = (
            SELECT COUNT(*)
            FROM chamcong ch
            WHERE TIMESTAMPDIFF(HOUR, gioTanLamHomQua, ThoiGianTanLam) >= 1
            AND ThoiGianTanLam = (
                SELECT ThoiGianTanLam
                FROM chamcong c
                WHERE c.IDNhanVien = ch.IDNhanVien
                AND DATE(ThoiGianVaoLam) = homQua
                ORDER BY ThoiGianTanLam DESC
                LIMIT 1
            )
        )
        WHERE NgayBaoCao = homQua;

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'Update baocaochamcong cho ngay da qua hoan tat', NULL, NULL, NULL);

        DROP TEMPORARY TABLE time5;
        DROP TEMPORARY TABLE time6;

        DROP TEMPORARY TABLE nhanvien_hientai;
        DROP TEMPORARY TABLE nv_ht2;
        DROP TEMPORARY TABLE nv_ht3;
        DROP TEMPORARY TABLE nv_ht4;
        DROP TEMPORARY TABLE nv_ht5;
        DROP TEMPORARY TABLE nv_ht6;

        /* !!!!LOG!!!! */
        INSERT INTO eventlog (LogTime, Message, HomQua, GioVaoLamHomQua, GioTanLamHomQua)
        VALUES (NOW(), 'KET THUC EVENT.', NULL, NULL, NULL);

/* endregion */
    END $

DELIMITER ;

DELIMITER $

DROP PROCEDURE IF EXISTS UPDATE_START_BUSINESS_HOURS;
CREATE PROCEDURE UPDATE_START_BUSINESS_HOURS()
BEGIN
    DECLARE businessHour datetime;
    SET businessHour = (
        SELECT
        ADDTIME(CURRENT_DATE() + INTERVAL 1 DAY,
            (CASE DAYOFWEEK(CURRENT_DATE() + INTERVAL 1 DAY)
                WHEN 1 THEN GioVaoLamChuNhat
                WHEN 7 THEN GioVaoLamThuBay
                ELSE GioVaoLamCacNgayTrongTuan
            END))
        FROM thoigianbieutuan
        WHERE ThoiDiemTao <= NOW()
        ORDER BY ThoiDiemTao DESC
        LIMIT 1
    );

    ALTER EVENT START_BUSINESS_HOURS_EVENT
    ON SCHEDULE
        EVERY 1 DAY
        STARTS businessHour
    ENABLE;
END $
DELIMITER ;

DELIMITER $

DROP EVENT IF EXISTS DAILY_UPDATE_START_BUSINESS_HOURS_EVENT;
CREATE EVENT DAILY_UPDATE_START_BUSINESS_HOURS_EVENT
ON SCHEDULE EVERY 1 DAY
STARTS (TIMESTAMP(CURRENT_DATE()) + INTERVAL 23 HOUR + INTERVAL 59 MINUTE /*+ INTERVAL 47 MINUTE*/)
DO
    BEGIN
        DECLARE homNay date;
        DECLARE coLamHomNay tinyint;

        SET homNay = CURRENT_DATE();
        SET coLamHomNay = (
            SELECT (
                CASE DAYOFWEEK(homNay)
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
        CREATE TEMPORARY TABLE currentHolidays
        SELECT *
        FROM nghile
        WHERE homNay BETWEEN MAKEDATE(YEAR(homNay), 1)
            + INTERVAL Thang - 1 MONTH
            + INTERVAL Ngay - 1 DAY
        AND MAKEDATE(YEAR(homNay), 1)
            + INTERVAL Thang - 1 MONTH
            + INTERVAL Ngay - 1 DAY
            + INTERVAL SoNgayNghi - 1 DAY;

        IF NOT EXISTS(SELECT * FROM currentHolidays) AND coLamHomNay = 1 THEN
            CALL UPDATE_START_BUSINESS_HOURS;
        ELSE
            ALTER EVENT START_BUSINESS_HOURS_EVENT
            DISABLE;
        END IF;
    END $

DELIMITER ;