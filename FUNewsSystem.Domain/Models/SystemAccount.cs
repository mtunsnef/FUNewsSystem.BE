using System;
using System.Collections.Generic;

namespace FUNewsSystem.Domain.Models;

public partial class SystemAccount
{
    public short AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; }

    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
