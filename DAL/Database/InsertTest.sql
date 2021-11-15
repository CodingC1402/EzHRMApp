DELETE FROM chamcong;
DELETE FROM luong;
DELETE FROM truluong;
DELETE FROM sogiolamtrongngay;
DELETE FROM nghiphep;
DELETE FROM nhanvien;
DELETE FROM phongban;
DELETE FROM cacloaivipham;
DELETE FROM chucvu;
DELETE FROM accesstokens;
DELETE FROM taikhoan;
DELETE FROM nhomtaikhoan;
DELETE FROM thamso;
DELETE FROM cachtinhluong;
DELETE FROM nghile;
DELETE FROM thoigianbieutuan;
DELETE FROM baocaochamcong;
DELETE FROM baocaonhansu;
DELETE FROM eventlog;


INSERT INTO thamso (ThoiDiemTao, CoLamThuHai, CoLamThuBa, CoLamThuTu,
                    CoLamThuNam, CoLamThuSau, CoLamThuBay, CoLamChuNhat,
                    ThoiGianChoPhepDiTre, ThoiGianChoPhepVeSom,
                    ThoiGianDiTreToiDa, ThoiGianVeSomToiDa)
VALUES (NOW() - INTERVAL 2 DAY, 1, 1, 1, 1, 1, 0, 0, '00:15:00', '00:20:00', '02:00:00', '1:00:00');

INSERT INTO thoigianbieutuan (ThoiDiemTao,
                              GioVaoLamCacNgayTrongTuan,
                              GioVaoLamThuBay,
                              GioVaoLamChuNhat,
                              GioTanLamCacNgayTrongTuan,
                              GioTanLamThuBay,
                              GioTanLamChuNhat)
VALUES (NOW() - INTERVAL 1 DAY,
        '07:00:00',
        '07:30:00',
        '00:00:00',
        '17:00:00',
        '16:00:00',
        '00:00:00');

INSERT INTO cacloaivipham (TenViPham, TruLuongTheoPhanTram, TruLuongTrucTiep)
VALUES ('DiTre', 2.0, 0), ('VeSom', 1.5, 0), ('VangMat', 5.0, 0);

INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan) VALUES ('boss',    0x0FFFFF);
INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan) VALUES ('hrm',     0x0000FF);
INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan) VALUES ('employee',0xF00000);

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('boss0', SHA2('0', 256), 'boss');

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('hrm0', SHA2('0', 256), 'hrm');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('hrm1', SHA2('1', 256), 'hrm');

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee0', SHA2('0', 256), 'employee');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee1', SHA2('1', 256), 'employee');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee2', SHA2('2', 256), 'employee');

INSERT INTO cachtinhluong (Ten, LanTraLuongCuoi, NgayTinhLuongThangNay)
    VALUES ('TheoThang', CURRENT_DATE() - INTERVAL 1 MONTH, CURRENT_DATE());
INSERT INTO cachtinhluong (Ten, KyHanTraLuongTheoNgay, LanTraLuongCuoi)
    VALUES ('TheoGio', 1, CURRENT_DATE() - INTERVAL 1 DAY);

INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, TienLuongMoiGio, TienLuongMoiThang, PhanTramLuongNgoaiGio)
    VALUES ('Designer', 'TheoThang', 0, 1500, 1.5);
INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, TienLuongMoiGio, TienLuongMoiThang, PhanTramLuongNgoaiGio)
    VALUES ('Programmer', 'TheoThang', 0, 1600, 1.5);
INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, TienLuongMoiGio, TienLuongMoiThang, PhanTramLuongNgoaiGio)
    VALUES ('Cleaner', 'TheoGio', 4, 0, 1.5);

INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('Design Department', null);
INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('Cleaning Department', null);
INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('Coding Department', null);

INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000001', 'Pham Phuc', 'Nguyen', '000000101010', '2001/02/14', 'NV000000001@gmail.com', 'Nguyen@gmail.com', '0938516968', '09384206968', '2021/04/04', null, 'Cleaning Department', 'Cleaner', 'employee0');
INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000002', 'Do Phi', 'Long', '000000101010', '2001/02/14', 'NV000000002@gmail.com', 'Long@gmail.com', '0938516968', '09384206968', '2021/04/04', null, 'Coding Department', 'Programmer', 'employee1');
INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000003', 'Nguyen Bao', 'Duy', '000000101010', '2001/02/14', 'NV000000003@gmail.com', 'Duy@gmail.com', '0938516968', '09384206968', '2021/04/04', null, 'Design Department', 'Designer', 'employee2');

INSERT INTO chamcong (ThoiGianVaoLam, IDNhanVien, ThoiGianTanLam)
VALUES (ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY, '07:00:00'), 'NV00000002', NULL);

INSERT INTO chamcong (ThoiGianVaoLam, IDNhanVien, ThoiGianTanLam)
VALUES (ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY, '11:00:00'), 'NV00000001', NULL);

INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
VALUES (CURRENT_DATE() - INTERVAL 1 DAY, 'NV00000001', 8, 8);

INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
VALUES (CURRENT_DATE() - INTERVAL 2 DAY, 'NV00000002', 0, 8);

INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
VALUES (CURRENT_DATE() - INTERVAL 2 DAY, 'NV00000003', 12, 8);

INSERT INTO truluong (Ngay, IDNhanVien, TenViPham, SoTienTru, SoPhanTramTru, GhiChu)
VALUES (CURRENT_DATE() - INTERVAL 2 DAY, 'NV00000001', 'VangMat', 100, 15, 'Testing...');

INSERT INTO truluong (Ngay, IDNhanVien, TenViPham, SoTienTru, SoPhanTramTru, GhiChu)
VALUES (CURRENT_DATE() - INTERVAL 2 DAY, 'NV00000002', 'VangMat', 80, 5, 'Testing...');
