DELIMITER  $$

DROP PROCEDURE IF EXISTS LOGIN;
CREATE PROCEDURE LOGIN(IN Username VARCHAR(255), IN LoginPassword VARCHAR(255), OUT AccessToken VARCHAR(255), OUT PrivilegeMask INT, OUT StaffID VARCHAR(10), OUT IsLoggedIn TINYINT, OUT Success TINYINT)
BEGIN
    DECLARE AccountCount TINYINT;
    DECLARE IsInUse TINYINT;
    DECLARE Bitmask INT;
    DECLARE NewToken VARCHAR(32);

    /** 1 = Succeeded 0 = failed */
    SET Success := 0;
    SELECT COUNT(*), DangLogIn, TK.QuyenHan INTO AccountCount, IsInUse, Bitmask FROM TAIKHOAN TK WHERE Username = TK.TaiKhoan AND LoginPassword = TK.Password;

    IF (AccountCount > 0) THEN
        SET Success := 1;
        SET StaffID := (SELECT ID FROM NHANVIEN WHERE TaiKhoan = Username LIMIT 1);
        SET PrivilegeMask := Bitmask;

        IF ( IsInUse = 0 OR IsInUse IS NULL) THEN
            UPDATE TAIKHOAN TK
            SET TK.DangLogin = 1
            WHERE TK.TaiKhoan = Username AND LoginPassword = TK.Password;

            SET NewToken := SUBSTRING(MD5(RAND()) ,1, 32);
            WHILE (EXISTS (SELECT * FROM ACCESSTOKENS WHERE Token = NewToken)) DO
                SET NewToken := SUBSTRING(MD5(RAND()) ,1, 32);
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
CREATE PROCEDURE LOGOUT(IN AccessToken VARCHAR(32))
BEGIN
    IF (EXISTS (SELECT * FROM AccessTokens WHERE Token = AccessToken)) THEN
        UPDATE TAIKHOAN TK
        SET TK.DangLogin = 0
        WHERE TK.TaiKhoan = (SELECT T.Account FROM AccessTokens T WHERE Token = AccessToken LIMIT 1);

        DELETE FROM ACCESSTOKENS WHERE AccessToken = Token;
    END IF;
END;
$$

DROP PROCEDURE IF EXISTS  LOGOUTALL;
CREATE PROCEDURE LOGOUTALL()
BEGIN
    DECLARE CurrentToken VARCHAR(32);

    WHILE( EXISTS(SELECT * FROM ACCESSTOKENS) ) DO
        SELECT Token INTO CurrentToken FROM ACCESSTOKENS LIMIT 1;
        CALL LOGOUT(CurrentToken);
    END WHILE;
END;
$$

DELIMITER ;

