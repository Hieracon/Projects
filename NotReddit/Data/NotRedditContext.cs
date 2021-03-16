using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotReddit.Models;

namespace NotReddit.Data
{
    public class NotRedditContext : IdentityDbContext
    {
        public NotRedditContext(DbContextOptions<NotRedditContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Subsection> Subsections { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Badge> Badges { get; set; }
    }
}
