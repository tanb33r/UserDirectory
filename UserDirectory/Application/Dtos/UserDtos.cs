namespace UserDirectory.Application.Dtos;

public record ContactDto
{
    public int Id { get; init; }
    public string Phone { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string City { get; init; } = default!;
    public string Country { get; init; } = default!;
}

public record RoleDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
}

public record UserDto
{
    public int Id { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public bool Active { get; init; }
    public string Company { get; init; } = default!;
    public string Sex { get; init; } = default!;

    public ContactDto Contact { get; init; } = default!;

    public RoleDto Role { get; init; } = default!;
}


public record CreateContactDto
{
    public string Phone { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string City { get; init; } = default!;
    public string Country { get; init; } = default!;
}


public record CreateUserDto
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public bool Active { get; init; }
    public string Company { get; init; } = default!;
    public string Sex { get; init; } = default!;

    public CreateContactDto Contact { get; init; } = default!;

    public int RoleId { get; init; }
}

public record UpdateUserDto : CreateUserDto
{
    public int Id { get; init; }
}