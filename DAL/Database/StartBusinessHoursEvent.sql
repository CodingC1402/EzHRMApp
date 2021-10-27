DELIMITER $

CREATE EVENT StartBusinessHours
ON SCHEDULE AT (
    SELECT
    CASE
        WHEN DAYOFWEEK(CURRENT_DATE() + INTERVAL 1 DAY) = 1 THEN GioVaoLamChuNhat
        WHEN DAYOFWEEK(CURRENT_DATE() + INTERVAL 1 DAY) = 2 THEN GioVaoLamThuHai
        WHEN DAYOFWEEK(CURRENT_DATE() + INTERVAL 1 DAY) = 3 THEN GioVaoLamThuBa
        WHEN DAYOFWEEK(CURRENT_DATE() + INTERVAL 1 DAY) = 4 THEN GioVaoLamThuTu
        WHEN DAYOFWEEK(CURRENT_DATE() + INTERVAL 1 DAY) = 5 THEN GioVaoLamThuNam
        WHEN DAYOFWEEK(CURRENT_DATE() + INTERVAL 1 DAY) = 6 THEN GioVaoLamThuSau
        WHEN DAYOFWEEK(CURRENT_DATE() + INTERVAL 1 DAY) = 7 THEN GioVaoLamThuBay
    END
    + INTERVAL 1 DAY
    FROM thoigianbieutuan
    WHERE DATE(ThoiDiemTao) <= CURRENT_DATE()
    ORDER BY ThoiDiemTao DESC
    LIMIT 1
    )
DO
    BEGIN
        CREATE TEMPORARY TABLE working_employees
        SELECT *
        FROM chamcong c
        WHERE DATE(ThoiGianVaoLam) = CURRENT_DATE() - INTERVAL 1 DAY
        AND ThoiGianTanLam IS NULL;

        UPDATE sogiolamtrongngay
        SET SoGioLamTrongGio = (
            IF ((SELECT
                CASE
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 1 THEN GioTanLamChuNhat
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 2 THEN GioTanLamThuHai
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 3 THEN GioTanLamThuBa
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 4 THEN GioTanLamThuTu
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 5 THEN GioTanLamThuNam
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 6 THEN GioTanLamThuSau
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 7 THEN GioTanLamThuBay
                END
                FROM thoigianbieutuan
                WHERE ThoiDiemTao <= CURRENT_DATE()
                ORDER BY ThoiDiemTao DESC
                LIMIT 1) > (SELECT ThoiGianVaoLam
                            FROM working_employees we
                            WHERE we.IDNhanVien = IDNhanVien)
                , SoGioLamTrongGio +
                HOUR(TIMEDIFF(
                (SELECT
                CASE
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 1 THEN GioTanLamChuNhat
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 2 THEN GioTanLamThuHai
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 3 THEN GioTanLamThuBa
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 4 THEN GioTanLamThuTu
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 5 THEN GioTanLamThuNam
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 6 THEN GioTanLamThuSau
                    WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 7 THEN GioTanLamThuBay
                END
                FROM thoigianbieutuan
                WHERE ThoiDiemTao <= CURRENT_DATE()
                ORDER BY ThoiDiemTao DESC
                LIMIT 1), (SELECT ThoiGianVaoLam
                            FROM working_employees we
                            WHERE we.IDNhanVien = IDNhanVien)))
                , SoGioLamTrongGio)
            ),
            SoGioLamNgoaiGio = (
                IF ((SELECT
                    CASE
                        WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 1 THEN GioTanLamChuNhat
                        WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 2 THEN GioTanLamThuHai
                        WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 3 THEN GioTanLamThuBa
                        WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 4 THEN GioTanLamThuTu
                        WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 5 THEN GioTanLamThuNam
                        WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 6 THEN GioTanLamThuSau
                        WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 7 THEN GioTanLamThuBay
                    END
                    FROM thoigianbieutuan
                    WHERE ThoiDiemTao <= CURRENT_DATE()
                    ORDER BY ThoiDiemTao DESC
                    LIMIT 1) > (SELECT ThoiGianVaoLam
                                FROM working_employees we
                                WHERE we.IDNhanVien = IDNhanVien)
                    , SoGioLamNgoaiGio +
                      HOUR(TIMEDIFF(
                          CURRENT_TIME(),
                          (SELECT
                        CASE
                            WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 1 THEN GioTanLamChuNhat
                            WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 2 THEN GioTanLamThuHai
                            WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 3 THEN GioTanLamThuBa
                            WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 4 THEN GioTanLamThuTu
                            WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 5 THEN GioTanLamThuNam
                            WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 6 THEN GioTanLamThuSau
                            WHEN DAYOFWEEK(CURRENT_DATE() - INTERVAL 1 DAY) = 7 THEN GioTanLamThuBay
                        END
                        FROM thoigianbieutuan
                        WHERE ThoiDiemTao <= CURRENT_DATE()
                        ORDER BY ThoiDiemTao DESC
                        LIMIT 1)))
                    , HOUR(TIMEDIFF(CURRENT_TIME(),
                            (SELECT ThoiGianVaoLam
                                    FROM working_employees we
                                    WHERE we.IDNhanVien = IDNhanVien))))
                )
        WHERE Ngay = CURRENT_DATE() - 1
        AND IDNhanVien IN (SELECT IDNhanVien FROM working_employees);

        UPDATE chamcong
        SET ThoiGianTanLam = CURRENT_TIME()
        WHERE IDNhanVien in (SELECT IDNhanVien FROM working_employees);

        INSERT INTO chamcong (ThoiGianVaoLam, IDNhanVien,  ThoiGianTanLam)
        SELECT NOW(), IDNhanVien, NULL
        FROM working_employees;

        INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
        SELECT CURRENT_DATE(), ID, 0, 0
        FROM nhanvien;

    END
    $

