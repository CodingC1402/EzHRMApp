show events;
show create event START_BUSINESS_HOURS_EVENT;

ALTER EVENT START_BUSINESS_HOURS_EVENT
ON SCHEDULE
EVERY 1 DAY
STARTS ADDTIME(CURRENT_DATE() + INTERVAL 1 DAY, '06:00:00')
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
SET ThoiGianVaoLam = '2021-11-14 11:00:00'
WHERE IDNhanVien = 'NV00000001';

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

SELECT * FROM NHANVIEN;
SELECT * FROM CHUCVU;
SELECT * FROM accesstokens;
SELECT * FROM TAIKHOAN;

INSERT INTO ACCESSTOKENS(Token, Bitmask, Account)
VALUES ('sfdasfadsf', Bitmask, 'CornyCornyCorn0');