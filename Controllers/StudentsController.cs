using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;

namespace ProductApi.Controllers;

// ============================================
// PRESENTATION LAYER - StudentsController
// ============================================
// Đây là Presentation Layer trong Clean Architecture
// Vai trò:
// - Nhận HTTP requests từ client (Postman, Browser, Mobile app)
// - Validate input cơ bản
// - Gọi Application Layer (StudentService) để xử lý business logic
// - Trả về HTTP response (200 OK, 404 Not Found, 400 Bad Request, etc.)
// 
// KHÔNG ĐƯỢC:
// - Truy cập trực tiếp Database (DbContext)
// - Chứa business logic phức tạp
// - Truy cập trực tiếp Repository
//
// Liên kết:
// - Phụ thuộc vào: Application.Services.StudentService
// - Được gọi bởi: HTTP Client (Postman, Swagger UI, Frontend)
// ============================================

[ApiController]  // Đánh dấu đây là API Controller, tự động validate model
[Route("api/[controller]")]  // Route: /api/students (controller name = Students)
public class StudentsController : ControllerBase
{
    // Dependency Injection: StudentService được inject vào controller
    // Không new() trực tiếp, giúp code dễ test và maintain
    private readonly StudentService _studentService;

    // Constructor Injection
    // ASP.NET Core tự động inject StudentService khi tạo controller
    // Service được đăng ký trong Program.cs
    public StudentsController(StudentService studentService)
    {
        _studentService = studentService;
    }

    // ============================================
    // GET: /api/students
    // ============================================
    // Mục đích: Lấy danh sách TẤT CẢ students
    // HTTP Method: GET
    // Cách hoạt động:
    // 1. Client gửi GET request đến /api/students
    // 2. Controller nhận request
    // 3. Gọi StudentService.GetAllStudentsAsync()
    // 4. Service gọi Repository để lấy data từ database
    // 5. Trả về danh sách students với status 200 OK
    //
    // Liên kết: Controller → StudentService → StudentRepository → Database
    // ============================================
    [HttpGet]  // Attribute định nghĩa HTTP GET method
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        // Gọi Service layer để lấy dữ liệu
        var students = await _studentService.GetAllStudentsAsync();
        
        // Trả về HTTP 200 OK kèm danh sách students
        return Ok(students);
    }

    // ============================================
    // GET: /api/students/{id}
    // ============================================
    // Mục đích: Lấy thông tin 1 student cụ thể theo ID
    // HTTP Method: GET
    // Parameter: id từ URL (ví dụ: /api/students/5)
    // 
    // Cách hoạt động:
    // 1. Client gửi GET /api/students/5
    // 2. ASP.NET Core tự động parse "5" thành parameter int id
    // 3. Controller gọi Service.GetStudentByIdAsync(5)
    // 4. Service validate và gọi Repository
    // 5. Repository query database WHERE Id = 5
    // 6. Nếu tìm thấy: return 200 OK + student data
    //    Nếu không: return 404 Not Found
    //
    // Response Codes:
    // - 200 OK: Tìm thấy student
    // - 404 Not Found: Không tồn tại student với ID này
    // ============================================
    [HttpGet("{id}")]  // Route parameter {id} từ URL
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        // Gọi Service để tìm student theo ID
        var student = await _studentService.GetStudentByIdAsync(id);
        
        // Kiểm tra nếu không tìm thấy
        if (student == null)
        {
            // Trả về HTTP 404 Not Found với message
            return NotFound($"Student with ID {id} not found.");
        }

        // Trả về HTTP 200 OK kèm student data
        return Ok(student);
    }

    // ============================================
    // POST: /api/students
    // ============================================
    // Mục đích: Tạo mới 1 student
    // HTTP Method: POST
    // Body: JSON object chứa student data
    // 
    // Example Request Body:
    // {
    //   "fullName": "John Doe",
    //   "email": "john@email.com",
    //   "dateOfBirth": "2000-01-15",
    //   "classId": 1
    // }
    //
    // Cách hoạt động:
    // 1. Client gửi POST request với JSON body
    // 2. ASP.NET Core tự động deserialize JSON → Student object
    // 3. [ApiController] tự động validate (ModelState)
    // 4. Controller gọi Service.CreateStudentAsync(student)
    // 5. Service validate business logic (email, classId, etc.)
    // 6. Repository insert vào database
    // 7. Trả về 201 Created với student mới tạo + Location header
    //
    // Response Codes:
    // - 201 Created: Tạo thành công, kèm URL của resource mới
    // - 400 Bad Request: Dữ liệu không hợp lệ
    //
    // Location Header: /api/students/{newId}
    // ============================================
    [HttpPost]  // HTTP POST method
    public async Task<ActionResult<Student>> CreateStudent(Student student)
    {
        // Gọi Service để tạo student mới
        // Service sẽ validate business rules
        var createdStudent = await _studentService.CreateStudentAsync(student);
        
        // Nếu validation fail (Service trả về null)
        if (createdStudent == null)
        {
            // Trả về HTTP 400 Bad Request
            return BadRequest("Invalid student data.");
        }

        // Trả về HTTP 201 Created
        // CreatedAtAction tự động tạo Location header: /api/students/{id}
        // nameof(GetStudent) = tên method để get student
        // new { id = createdStudent.Id } = route values
        // createdStudent = response body
        return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
    }

    // ============================================
    // PUT: /api/students/{id}
    // ============================================
    // Mục đích: Cập nhật thông tin student đã tồn tại
    // HTTP Method: PUT
    // Parameter: id từ URL
    // Body: JSON object với student data đầy đủ
    //
    // Example Request:
    // PUT /api/students/5
    // Body: {
    //   "id": 5,
    //   "fullName": "John Updated",
    //   "email": "john.new@email.com",
    //   "dateOfBirth": "2000-01-15",
    //   "classId": 2
    // }
    //
    // Cách hoạt động:
    // 1. Client gửi PUT request với id trong URL và full data trong body
    // 2. Controller check: id từ URL === id trong body
    // 3. Gọi Service.UpdateStudentAsync(student)
    // 4. Service validate và gọi Repository
    // 5. Repository tìm student trong DB và update
    // 6. Trả về 200 OK với student đã update
    //
    // Response Codes:
    // - 200 OK: Update thành công
    // - 400 Bad Request: ID mismatch hoặc data không hợp lệ
    // - 404 Not Found: Student không tồn tại
    //
    // Note: PUT yêu cầu gửi TOÀN BỘ object (không phải chỉ field thay đổi)
    // ============================================
    [HttpPut("{id}")]  // HTTP PUT method với route parameter
    public async Task<ActionResult<Student>> UpdateStudent(int id, Student student)
    {
        // Security check: ID trong URL phải khớp với ID trong body
        // Tránh trường hợp update nhầm student
        if (id != student.Id)
        {
            // Trả về HTTP 400 Bad Request
            return BadRequest("ID mismatch.");
        }

        // Gọi Service để update student
        var updatedStudent = await _studentService.UpdateStudentAsync(student);
        
        // Nếu student không tồn tại (Service trả về null)
        if (updatedStudent == null)
        {
            // Trả về HTTP 404 Not Found
            return NotFound($"Student with ID {id} not found.");
        }

        // Trả về HTTP 200 OK với student đã update
        return Ok(updatedStudent);
    }

    // ============================================
    // DELETE: /api/students/{id}
    // ============================================
    // Mục đích: Xóa student khỏi database
    // HTTP Method: DELETE
    // Parameter: id từ URL
    //
    // Example Request:
    // DELETE /api/students/5
    //
    // Cách hoạt động:
    // 1. Client gửi DELETE request với ID trong URL
    // 2. Controller gọi Service.DeleteStudentAsync(id)
    // 3. Service gọi Repository
    // 4. Repository tìm student và xóa khỏi database
    // 5. Nếu xóa thành công: return 204 No Content
    //    Nếu không tìm thấy: return 404 Not Found
    //
    // Response Codes:
    // - 204 No Content: Xóa thành công (không có body)
    // - 404 Not Found: Student không tồn tại
    //
    // Note: 204 No Content là chuẩn REST cho DELETE thành công
    // Không trả về data vì resource đã không còn tồn tại
    // ============================================
    [HttpDelete("{id}")]  // HTTP DELETE method
    public async Task<ActionResult> DeleteStudent(int id)
    {
        // Gọi Service để xóa student
        var deleted = await _studentService.DeleteStudentAsync(id);
        
        // Nếu không tìm thấy student để xóa
        if (!deleted)
        {
            // Trả về HTTP 404 Not Found
            return NotFound($"Student with ID {id} not found.");
        }

        // Trả về HTTP 204 No Content (xóa thành công, không có response body)
        return NoContent();
    }
}