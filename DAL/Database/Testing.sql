INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan) VALUES ('boss',    0x0FFFFF);
INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan) VALUES ('hrm',     0x0000FF);
INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan) VALUES ('employee',0xF00000);

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('boss0', SHA2('0', 256), 'boss');

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('hrm0', SHA2('0', 256), 'hrm');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('hrm1', SHA2('1', 256), 'hrm');

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee0', SHA2('0', 256), 'employee');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee1', SHA2('1', 256), 'employee');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee2', SHA2('2', 256), 'employee');

INSERT INTO cachtinhluong (Ten)
    VALUES ('PieceWork');
INSERT INTO cachtinhluong (Ten)
    VALUES ('Commission');
INSERT INTO cachtinhluong (Ten)
    VALUES ('Salary');
INSERT INTO cachtinhluong (Ten)
    VALUES ('Wage');

INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, MucLuongNgoaiGio)
    VALUES ('Designer', 'Salary', 1.5);
INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, MucLuongNgoaiGio)
    VALUES ('Programmer', 'Salary', 1.5);
INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, MucLuongNgoaiGio)
    VALUES ('Cleaner', 'Wage', 1.5);

INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('Design Department', null);
INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('Cleaning Department', null);
INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('Coding Department', null);

INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000001', 'Pham Phuc', 'Nguyen', '000000101010', '2001/02/14', 'NV000000001@gmail.com', 'Nguyen@gmail.com', '0938516968', '09384206968', '2021/04/04', null, 3, 7, 'employee0');
INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000002', 'Do Phi', 'Long', '000000101010', '2001/02/14', 'NV000000002@gmail.com', 'Long@gmail.com', '0938516968', '09384206968', '2021/04/04', null, 3, 7, 'employee1');



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
DELETE FROM TAIKHOAN;

SELECT * FROM NHANVIEN;
SELECT * FROM CHUCVU;
SELECT * FROM accesstokens;
SELECT * FROM TAIKHOAN;

INSERT INTO ACCESSTOKENS(Token, Bitmask, Account)
VALUES ('sfdasfadsf', Bitmask, 'CornyCornyCorn0');