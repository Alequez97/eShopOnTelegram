﻿namespace eShopOnTelegram.Persistence.Entities;

public class EntityBase
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}
