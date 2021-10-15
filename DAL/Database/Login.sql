DELIMITER  $$

DROP PROCEDURE IF EXISTS LOGIN;
CREATE PROCEDURE LOGIN(IN Username VARCHAR(255), IN LoginPassword VARCHAR(255), OUT AccessToken VARCHAR(255), OUT PrivilegeMask INT, OUT StaffID VARCHAR(10), OUT IsLoggedIn TINYINT)
BEGIN
    DECLARE AccountCount TINYINT;
    DECLARE IsInUse TINYINT;
    DECLARE Bitmask INT;
    DECLARE NewToken VARCHAR(255);

    SELECT COUNT(*), DangLogIn, TK.QuyenHan INTO AccountCount, IsInUse, Bitmask FROM TAIKHOAN TK WHERE Username = TK.TaiKhoan AND LoginPassword = TK.Password;
    IF (AccountCount > 0) THEN
        SET StaffID := (SELECT ID FROM NHANVIEN WHERE TaiKhoan = Username LIMIT 1);
        SET PrivilegeMask := Bitmask;

        IF ( IsInUse = 0 OR IsInUse IS NULL) THEN
            UPDATE TAIKHOAN TK
            SET TK.DangLogin = 1
            WHERE TK.TaiKhoan = Username AND LoginPassword = TK.Password;

            SET NewToken := RAND();
            WHILE (EXISTS (SELECT * FROM ACCESSTOKENS WHERE Token = NewToken)) DO
                SET NewToken := RAND();
            END WHILE;

            INSERT INTO ACCESSTOKENS(Token, Bitmask, NhanVienID, Account)
            VALUES (NewToken, Bitmask, StaffID, Username);

            SET AccessToken := NewToken;
            SET IsLoggedIn := 0;
        ELSE
            SELECT t.Token INTO AccessToken FROM ACCESSTOKENS t WHERE Account = Username;
            SET IsLoggedIn := 1;
        END IF;
    END IF;
END;
$$

DROP PROCEDURE IF EXISTS  LOGOUT;
CREATE PROCEDURE LOGOUT(IN Username VARCHAR(255), IN LoginPassword VARCHAR(255))
BEGIN
    DECLARE AccountCount TINYINT;

    SELECT COUNT(*) INTO AccountCount FROM TAIKHOAN TK WHERE Username = TK.TaiKhoan AND LoginPassword = TK.Password;
    IF (AccountCount > 0) THEN
        UPDATE TAIKHOAN TK
        SET TK.DangLogin = 0
        WHERE TK.TaiKhoan = Username AND LoginPassword = TK.Password;

        DELETE FROM ACCESSTOKENS WHERE Account = Username;
    END IF;
END;
$$

DELIMITER ;

