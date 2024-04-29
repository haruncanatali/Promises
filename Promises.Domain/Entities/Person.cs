using Promises.Domain.Base;

namespace Promises.Domain.Entities;

public class Person : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Photo { get; set; }

    public List<Agreement> Agreements { get; set; }
}