INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan) VALUES ('boss',    0x0FFFFF);
INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan) VALUES ('hrm',     0x0000FF);
INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan) VALUES ('employee',0xF00000);

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('boss0', SHA2('0', 256), 'boss');

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('hrm0', SHA2('0', 256), 'hrm');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('hrm1', SHA2('1', 256), 'hrm');

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee0', SHA2('0', 256), 'employee');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee1', SHA2('1', 256), 'employee');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee2', SHA2('2', 256), 'employee');

DELIMITER  $$
CALL LOGIN('boss0', SHA2('0', 256), @AccessToken, @Mask, @ID, @IsLogedIn, @Success);
SELECT @AccessToken;
SELECT @Mask;
SELECT @ID;
SELECT @IsLogedIn;
$$
DELIMITER ;

CALL LOGOUT('6da463e7ad28a4cf8d36feb94e46a4fd');
CALL LOGOUTALL();

 UPDATE TAIKHOAN TK
        SET TK.DangLogin = 0
        WHERE TK.TaiKhoan = 'boss0';

 UPDATE TAIKHOAN TK
        SET TK.DangLogin = 0
        WHERE TK.TaiKhoan = 'CornyCornyCorn1';

DELETE FROM accesstokens;
DELETE FROM TAIKHOAN;
SELECT * FROM accesstokens;
SELECT * FROM TAIKHOAN;

INSERT INTO ACCESSTOKENS(Token, Bitmask, Account)
VALUES ('sfdasfadsf', Bitmask, 'CornyCornyCorn0');