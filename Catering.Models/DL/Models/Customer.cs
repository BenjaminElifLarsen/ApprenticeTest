﻿using Catering.Models.DL.Models;
using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

public class Customer : IAggregateRoot
{
    private Guid _id;
    private CustomerLocation _location;
    private HashSet<ReferenceId> _orders;

    public Guid Id { get => _id; private set => _id = value; }
    public CustomerLocation Location { get => _location; private set => _location = value; }
    public IEnumerable<ReferenceId> Orders { get => _orders; private set => _orders = value.ToHashSet(); }

    private Customer()
    {
        
    }

    internal Customer(Guid customerId, CustomerLocation location)
    {
        _id = customerId;
        _location = location;
        _orders = [];
    }
}
