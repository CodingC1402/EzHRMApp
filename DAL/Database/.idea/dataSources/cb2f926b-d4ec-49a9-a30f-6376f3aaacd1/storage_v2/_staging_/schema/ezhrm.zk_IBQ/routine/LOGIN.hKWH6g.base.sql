create
    definer = root@localhost procedure LOGIN(IN Username varchar(255), IN LoginPassword varchar(255),
                                             OUT AccessToken varchar(255), OUT PrivilegeMask int,
                                             OUT StaffID varchar(10), OUT IsLoggedIn tinyint, OUT Success tinyint)
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

