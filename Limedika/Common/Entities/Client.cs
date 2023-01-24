﻿using LinqToDB.Mapping;

namespace Common.Entities
{        
    public class Client
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
    }
}
