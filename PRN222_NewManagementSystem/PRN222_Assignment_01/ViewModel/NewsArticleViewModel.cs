namespace PRN222_Assignment_01.ViewModel
{
    public class NewsArticleViewModel
    {
        public int NewsArticleID { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string NewsSource { get; set; }
        public string NewsContent { get; set; }
        public bool? NewsStatus { get; set; }

        public short? CategoryID { get; set; }   // Lưu ID khi chọn
        public string CategoryName { get; set; } // Hiển thị tên (không cần dùng ở dropdown)

        public string CreatedByName { get; set; }
        public short? CreatedByID { get; set; }
        public string UpdatedByName { get; set; }
        public short? UpdatedByID { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}
