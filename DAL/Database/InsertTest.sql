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

INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan, DaXoa) VALUES ('boss',    0x0FFFFF, 0);
INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan, DaXoa) VALUES ('hrm',     0x0000FF, 0);
INSERT INTO NHOMTAIKHOAN (TenNhomTaiKHoan, QuyenHan, DaXoa) VALUES ('employee',0xF00000, 0);

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('boss0', SHA2('0', 256), 'boss');

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('hrm0', SHA2('0', 256), 'hrm');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('hrm1', SHA2('1', 256), 'hrm');

INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee0', SHA2('0', 256), 'employee');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee1', SHA2('1', 256), 'employee');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee2', SHA2('2', 256), 'employee');
INSERT INTO TAIKHOAN (TaiKhoan, Password, NhomTaiKhoan) VALUES ('employee3', SHA2('3', 256), 'employee');

INSERT INTO cachtinhluong (Ten, LanTraLuongCuoi, NgayTinhLuongThangNay)
    VALUES ('TheoThang', CURRENT_DATE() - INTERVAL 1 MONTH, CURRENT_DATE());
INSERT INTO cachtinhluong (Ten, KyHanTraLuongTheoNgay, LanTraLuongCuoi)
    VALUES ('TheoGio', 1, CURRENT_DATE() - INTERVAL 1 DAY);
INSERT INTO cachtinhluong (Ten, KyHanTraLuongTheoNgay, LanTraLuongCuoi, NgayTinhLuongThangNay)
    VALUES ('TheoTuan', 1, CURRENT_DATE() - INTERVAL 7 DAY, CURRENT_DATE());

INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, TienLuongMoiGio, TienLuongMoiThang, PhanTramLuongNgoaiGio, NhomTaiKhoan)
    VALUES ('HR Manager', 'TheoThang', 0, 1800, 1.5, 'hrm');
INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, TienLuongMoiGio, TienLuongMoiThang, PhanTramLuongNgoaiGio, NhomTaiKhoan)
    VALUES ('Designer', 'TheoThang', 0, 1500, 1.5, 'employee');
INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, TienLuongMoiGio, TienLuongMoiThang, PhanTramLuongNgoaiGio, NhomTaiKhoan)
    VALUES ('Programmer', 'TheoThang', 0, 1600, 1.5, 'employee');
INSERT INTO CHUCVU (TenChucVu, CachTinhLuong, TienLuongMoiGio, TienLuongMoiThang, PhanTramLuongNgoaiGio, NhomTaiKhoan)
    VALUES ('Cleaner', 'TheoGio', 4, 0, 1.5, 'employee');

INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('HR Department', null);
INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('Design Department', null);
INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('Cleaning Department', null);
INSERT INTO PHONGBAN (TenPhong, TruongPhong)
    VALUES ('Coding Department', null);

INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000000', 'Thundercok', 'Chad', '111111111111', '2010/4/20', 'NV000000000@gmail.com', 'Chad@gmail.com', '0969069420', '0969069420', '2020/04/20', null, 'HR Department', 'HR Manager', 'hrm0');
INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000001', 'Pham Phuc', 'Nguyen', '000000101010', '2001/02/14', 'NV000000001@gmail.com', 'Nguyen@gmail.com', '0938516968', '09384206968', '2021/04/04', null, 'Cleaning Department', 'Cleaner', 'employee0');
INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000002', 'Do Phi', 'Long', '000000101010', '2001/02/14', 'NV000000002@gmail.com', 'Long@gmail.com', '0938516968', '09384206968', '2021/04/04', null, 'Coding Department', 'Programmer', 'employee1');
INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000003', 'Nguyen Bao', 'Duy', '000000101010', '2001/02/14', 'NV000000003@gmail.com', 'Duy@gmail.com', '0938516968', '09384206968', '2021/04/04', null, 'Design Department', 'Designer', 'employee2');
INSERT INTO NHANVIEN (ID, Ho, Ten, CMND, NgaySinh, EmailVanPhong, EmailCaNhan, SDTVanPhong, SDTCaNhan, NgayVaoLam, NgayThoiViec, PhongBan, ChucVu, TaiKhoan)
    VALUES ('NV00000004', 'Nguyen Vuong Thanh', 'Tuan', '000000101010', '2001/02/14', 'NV000000004@gmail.com', 'Tuan@gmail.com', '0938516968', '09384206968', '2021/04/04', null, 'Design Department', 'Designer', 'employee3');

INSERT INTO chamcong (ThoiGianVaoLam, IDNhanVien, ThoiGianTanLam)
VALUES (ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY, '07:00:00'), 'NV00000002', ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY, '17:00:00'));

INSERT INTO chamcong (ThoiGianVaoLam, IDNhanVien, ThoiGianTanLam)
VALUES (ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY, '11:00:00'), 'NV00000001', ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY, '15:55:00'));

INSERT INTO chamcong (ThoiGianVaoLam, IDNhanVien, ThoiGianTanLam)
VALUES (ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY, '07:15:00'), 'NV00000003', ADDTIME(CURRENT_DATE() - INTERVAL 1 DAY, '21:00:00'));

INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
VALUES (CURRENT_DATE() - INTERVAL 2 DAY, 'NV00000001', 8, 8);

INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
VALUES (CURRENT_DATE() - INTERVAL 2 DAY, 'NV00000002', 0, 8);

INSERT INTO sogiolamtrongngay (Ngay, IDNhanVien, SoGioLamTrongGio, SoGioLamNgoaiGio)
VALUES (CURRENT_DATE() - INTERVAL 2 DAY, 'NV00000003', 12, 8);

INSERT INTO truluong (Ngay, IDNhanVien, TenViPham, SoTienTru, SoPhanTramTru, GhiChu)
VALUES (CURRENT_DATE() - INTERVAL 1 DAY, 'NV00000001', 'VangMat', 100, 15, 'Testing...');

INSERT INTO truluong (Ngay, IDNhanVien, TenViPham, SoTienTru, SoPhanTramTru, GhiChu)
VALUES (CURRENT_DATE() - INTERVAL 2 DAY, 'NV00000002', 'VangMat', 80, 5, 'Testing...');

INSERT INTO truluong (Ngay, IDNhanVien, TenViPham, SoTienTru, SoPhanTramTru, GhiChu)
VALUES (CURRENT_DATE() - INTERVAL 2 DAY, 'NV00000003', 'VangMat', 80, 5, 'Testing a very very very very very very long paragraph for checking the text box if it works or not');

INSERT INTO nghiphep (IDNhanVien, NgayBatDauNghi, SoNgayNghi, LyDoNghi, CoPhep)
VALUES ('NV00000001', CURRENT_DATE() - INTERVAL 1 DAY, 1, 'Tham gia hoi nghi 3N Quoc Te', 1);

INSERT INTO nghiphep (IDNhanVien, NgayBatDauNghi, SoNgayNghi, LyDoNghi, CoPhep)
VALUES ('NV00000002', CURRENT_DATE() - INTERVAL 1 DAY, 1, 'Tai nan giao thong', 1);

INSERT INTO baocaochamcong (NgayBaoCao, SoNVDenSom, SoNVDenDungGio, SoNVDenTre, SoNVTanLamSom, SoNVTanLamDungGio, SoNVLamThemGio, SoNVVangMat)
VALUES ('2021/11/15', 10, 40, 12, 0, 45, 17, 0),
('2021/11/16', 14, 35, 13, 5, 40, 17, 5),
('2021/11/17', 13, 42, 7, 4, 40, 18, 6),
('2021/11/18', 13, 37, 12, 20, 25, 16, 2);

INSERT INTO baocaonhansu (Thang, Nam, SoNhanVienMoi, SoNhanVienThoiViec)
VALUES (1, 2021, 10, 20),
       (2, 2021, 12, 10),
       (3, 2021, 2, 1),
       (4, 2021, 1, 7),
       (5, 2021, 4, 0),
       (6, 2021, 3, 1),
       (7, 2021, 1, 2),
       (8, 2021, 1, 10),
       (9, 2021, 12, 1),
       (10, 2021, 20, 1)