﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PRN222_Assignment_01.Models;

[PrimaryKey("NewsArticleID", "TagID")]
[Table("NewsTag")]
public partial class NewsTag
{
    [Key]
    public int NewsArticleID { get; set; }

    [Key]
    public int TagID { get; set; }

    [ForeignKey("NewsArticleID")]
    [InverseProperty("NewsTags")]
    public virtual NewsArticle NewsArticle { get; set; }

    [ForeignKey("TagID")]
    [InverseProperty("NewsTags")]
    public virtual Tag Tag { get; set; }
}