INSERT INTO TAIKHOAN (TaiKhoan, Password, QuyenHan) VALUES ('CornyCornyCorn0', SHA2('Password123', 256), 123);
INSERT INTO TAIKHOAN (TaiKhoan, Password, QuyenHan) VALUES ('CornyCornyCorn1', SHA2('Password124', 256), 126);
INSERT INTO TAIKHOAN (TaiKhoan, Password, QuyenHan) VALUES ('CornyCornyCorn2', SHA2('Password125', 256), 127);

DELIMITER  $$
CALL LOGIN('CornyCornyCorn0', 'Password123', @AccessToken, @Mask, @ID, @IsLogedIn, @Success);
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
        WHERE TK.TaiKhoan = 'CornyCornyCorn0';

 UPDATE TAIKHOAN TK
        SET TK.DangLogin = 0
        WHERE TK.TaiKhoan = 'CornyCornyCorn1';

DELETE FROM accesstokens;
SELECT * FROM accesstokens;
SELECT * FROM TAIKHOAN;

INSERT INTO ACCESSTOKENS(Token, Bitmask, Account)
VALUES ('sfdasfadsf', Bitmask, 'CornyCornyCorn0');