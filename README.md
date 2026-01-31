# Pickleball Club Management System

**Thông tin dự án**
- **Sinh viên**: Lễ Thế Duy
- **MSV**: 1871020186
- **Năm học**: 2025-2026

## Giới thiệu

Ứng dụng quản lý câu lạc bộ pickleball giúp quản lý đặt sân, trận đấu, thách đấu, hồ sơ thành viên và tài chính.

## Tính năng

- **Quản lý thành viên**: Đăng ký, đăng nhập, hồ sơ cá nhân
- **Đặt sân**: Đặt sân chơi, quản lý lịch sử
- **Trận đấu**: Tổ chức trận đấu, xem lịch sử
- **Thách đấu**: Tạo thách đấu giữa các thành viên
- **Tài chính**: Quản lý giao dịch, chi phí
- **Tin tức**: Chia sẻ thông tin câu lạc bộ

## Công nghệ

- **Framework**: ASP.NET Core 10.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core 10.0.2
- **UI**: Razor Pages

## Cài đặt

### 1. Chuẩn bị
```bash
# Kiểm tra phiên bản .NET
dotnet --version

# Phải có SQL Server chạy
```

### 2. Cài đặt Package
```bash
dotnet restore
```

### 3. Cấu hình Database
Sửa file `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PickleballClubManagement;Trusted_Connection=true;"
  }
}
```

### 4. Tạo Database
```bash
dotnet ef database update
```

### 5. Chạy ứng dụng
```bash
dotnet run
```

Truy cập: `https://localhost:5001`

## Cấu trúc thư mục

```
├── Models/              # Entity classes
├── Pages/               # Razor Pages (giao diện)
├── Services/            # Business logic
├── Data/                # Database context
├── Migrations/          # Database migrations
└── wwwroot/             # Static files (CSS, JS)
```

## Các Service

- **BookingService**: Quản lý đặt sân
- **MatchService**: Quản lý trận đấu
- **ChallengeService**: Quản lý thách đấu
- **MemberService**: Quản lý thành viên

## Cơ sở dữ liệu

**Các bảng chính:**
- Members - Thành viên
- Courts - Sân chơi
- Bookings - Đặt sân
- Matches - Trận đấu
- Challenges - Thách đấu
- Transactions - Giao dịch
- News - Tin tức

## Xử lý lỗi

**Lỗi kết nối database:**
```bash
# Kiểm tra SQL Server chạy
# Kiểm tra connection string
# Thử: dotnet ef database drop --force
#      dotnet ef database update
```

**Lỗi port đang sử dụng:**
```bash
dotnet run --urls "https://localhost:5002"
```

**Lỗi thiếu package:**
```bash
dotnet restore
dotnet clean
dotnet build
```

## Đặc điểm bảo mật

- Mã hóa mật khẩu
- Xác thực người dùng
- CSRF protection
- SQL injection prevention
- XSS protection

## Phát triển

### Quy ước đặt tên
- Class: PascalCase (BookingService)
- Method: PascalCase (GetBooking)
- Variable: camelCase (memberCount)

### Workflow
1. Tạo branch mới: `git checkout -b feature/tên-tính-năng`
2. Viết code
3. Commit: `git commit -m "Mô tả thay đổi"`
4. Push: `git push origin feature/tên-tính-năng`
5. Merge vào main

## Triển khai

```bash
# Build release
dotnet publish --configuration Release --output ./publish

# Copy folder publish lên server
# Cấu hình IIS hoặc Nginx
# Chạy migrations trên server
```

 
