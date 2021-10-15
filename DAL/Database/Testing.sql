INSERT INTO TAIKHOAN (TaiKhoan, Password, QuyenHan) VALUES ('CornyCornyCorn0', 'Password123', 123);
INSERT INTO TAIKHOAN (TaiKhoan, Password, QuyenHan) VALUES ('CornyCornyCorn1', 'Password124', 126);
INSERT INTO TAIKHOAN (TaiKhoan, Password, QuyenHan) VALUES ('CornyCornyCorn2', 'Password125', 127);

DELIMITER  $$
CALL LOGIN('CornyCornyCorn1', 'Password124', @AccessToken, @Mask, @ID, @IsLogedIn);
SELECT @AccessToken;
SELECT @Mask;
SELECT @ID;
SELECT @IsLogedIn;
$$
DELIMITER ;

CALL LOGOUT('CornyCornyCorn0', 'Password123');

DELETE FROM accesstokens;
SELECT * FROM accesstokens;
SELECT * FROM TAIKHOAN;

INSERT INTO ACCESSTOKENS(Token, Bitmask, Account)
VALUES ('sfdasfadsf', Bitmask, 'CornyCornyCorn0');