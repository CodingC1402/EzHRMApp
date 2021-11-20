show events;
show create event START_BUSINESS_HOURS_EVENT;

ALTER EVENT START_BUSINESS_HOURS_EVENT
ON SCHEDULE
EVERY 1 DAY
STARTS NOW() + INTERVAL 10 SECOND
ENABLE;

select * from eventlog;

select * from thamso;
select * from cacloaivipham;
select * from thoigianbieutuan;
select * from cachtinhluong;
select * from nhanvien;
select * from chamcong;
select * from chucvu;
select * from truluong;
select * from luong;
show events;
show create event START_BUSINESS_HOURS_EVENT;

ALTER EVENT START_BUSINESS_HOURS_EVENT
ON SCHEDULE
EVERY 1 DAY
STARTS NOW() + INTERVAL 10 SECOND
ENABLE;

select * from eventlog;

select * from thamso;
select * from cacloaivipham;
select * from thoigianbieutuan;
select * from cachtinhluong;
select * from nhanvien;
select * from chamcong;
select * from chucvu;
select * from truluong;
select * from luong;
select * from nghiphep;
select * from sogiolamtrongngay;
select * from baocaochamcong;
select * from baocaonhansu;

DELETE FROM sogiolamtrongngay;
DELETE FROM nghiphep;
DELETE FROM chamcong;
DELETE FROM truluong;
DELETE FROM luong;

UPDATE thoigianbieutuan
SET GioVaoLamChuNhat = '07:00:00';
UPDATE thoigianbieutuan
SET GioTanLamChuNhat = '17:00:00';

UPDATE chamcong
SET ThoiGianTanLam = '2021-11-15 18:00:00'
WHERE IDNhanVien = 'NV00000001';

UPDATE sogiolamtrongngay
SET SoGioLamTrongGio = 6.5
WHERE IDNhanVien = 'NV00000001';

INSERT INTO baocaochamcong (NgayBaoCao,
                            SoNVDenSom,
                            SoNVDenDungGio,
                            SoNVDenTre,
                            SoNVTanLamSom,
                            SoNVTanLamDungGio,
                            SoNVLamThemGio,
                            SoNVVangMat)
VALUES (CURRENT_DATE() - INTERVAL 1 DAY, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO baocaonhansu (Thang, Nam, SoNhanVienMoi, SoNhanVienThoiViec)
VALUES (MONTH(CURRENT_DATE()), YEAR(CURRENT_DATE()), 0, 0);


/*   test queries   */



DELIMITER  $$
CALL LOGIN('boss0', SHA2('0', 256), @AccessToken, @Mask, @ID, @IsLogedIn, @Success);
SELECT @AccessToken;
SELECT @Mask;
SELECT @ID;
SELECT @IsLogedIn;
$$
DELIMITER ;

USE `EzHRM` ;
CALL LOGOUT('6da463e7ad28a4cf8d36feb94e46a4fd');
CALL LOGOUTALL();

 UPDATE TAIKHOAN TK
        SET TK.DangLogin = 0
        WHERE TK.TaiKhoan = 'boss0';

 UPDATE TAIKHOAN TK
        SET TK.DangLogin = 0
        WHERE TK.TaiKhoan = 'CornyCornyCorn1';

DELETE FROM chucvu;
DELETE FROM accesstokens;
DELETE FROM taikhoan;
DELETE FROM nhomtaikhoan;

SELECT * FROM baocaochamcong;
SELECT * FROM NHANVIEN;
SELECT * FROM CHUCVU;
SELECT * FROM accesstokens;
SELECT * FROM TAIKHOAN;
SELECT * FROM TRULUONG;

INSERT INTO ACCESSTOKENS(Token, Bitmask, Account)
VALUES ('sfdasfadsf', Bitmask, 'CornyCornyCorn0');