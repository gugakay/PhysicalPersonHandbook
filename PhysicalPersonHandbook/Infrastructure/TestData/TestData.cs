using DataAccess.Entities;
using DataAccess.Enums;

namespace PhysicalPersonHandbook.Infrastructure.TestData
{
    public class TestData
    {
        public static List<Person> GetPersons()
        {
            return new List<Person>
            {
                new Person
                {
                    Name = "John",
                    LastName = "Doe",
                    Gender = Gender.Male,
                    PrivateNumber = "12345678901",
                    BirthDate = new DateTime(1980, 1, 1),
                    CityCode = 1,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber
                        {
                            Number = "111111111",
                            Type = PhoneNumberType.Mobile
                        },
                        new PhoneNumber
                        {
                            Number = "122222222",
                            Type = PhoneNumberType.Home
                        }
                    },
                    ConnectedPersons = new List<ConnectedPerson>
                    {
                        new ConnectedPerson
                        {
                            PersonId = 1,
                            ConnectedPersonId = 2,
                            Type = PersonConnectionType.Acquaintance
                        }
                    }
                },
                new Person
                {
                    Name = "Jane",
                    LastName = "Doe",
                    Gender = Gender.Female,
                    PrivateNumber = "12345678902",
                    BirthDate = new DateTime(1980, 1, 1),
                    CityCode = 1,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber
                        {
                            Number = "211111111",
                            Type = PhoneNumberType.Mobile
                        },
                        new PhoneNumber
                        {
                            Number = "222222222",
                            Type = PhoneNumberType.Home
                        }
                    },
                    ConnectedPersons = new List<ConnectedPerson>
                    {
                        new ConnectedPerson
                        {
                            PersonId = 2,
                            ConnectedPersonId = 1,
                            Type = PersonConnectionType.Acquaintance
                        }
                    }
                },
                new Person
                {
                    Name = "John",
                    LastName = "Smith",
                    Gender = Gender.Male,
                    PrivateNumber = "12345678903",
                    BirthDate = new DateTime(1980, 1, 1),
                    CityCode = 2,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber
                        {
                            Number = "311111111",
                            Type = PhoneNumberType.Mobile
                        },
                        new PhoneNumber
                        {
                            Number = "322222222",
                            Type = PhoneNumberType.Home
                        }
                    }
                },
                new Person
                {
                    Name = "Jane",
                    LastName = "Smith",
                    Gender = Gender.Female,
                    PrivateNumber = "12345678904",
                    BirthDate = new DateTime(1980, 1, 1),
                    CityCode = 2
                },
                new Person
                {
                    Name = "Name1",
                    LastName = "LastName1",
                    Gender = Gender.Male,
                    PrivateNumber = "12345678905",
                    BirthDate = new DateTime(1980, 1, 1),
                    CityCode = 3
                },
                new Person
                {
                    Name = "Name2",
                    LastName = "LastName2",
                    Gender = Gender.Male,
                    PrivateNumber = "12345678906",
                    BirthDate = new DateTime(1980, 1, 1),
                    CityCode = 3
                },
                new Person
                {
                    Name = "Name3",
                    LastName = "LastName3",
                    Gender = Gender.Female,
                    PrivateNumber = "12345678907",
                    BirthDate = new DateTime(1980, 1, 1),
                    CityCode = 3
                },
                new Person
                {
                    Name = "Name4",
                    LastName = "LastName4",
                    Gender = Gender.Female,
                    PrivateNumber = "12345678908",
                    BirthDate = new DateTime(1980, 1, 1),
                    CityCode = 4
                },
                new Person
                {
                    Name = "Name5",
                    LastName = "LastName5",
                    Gender = Gender.Female,
                    PrivateNumber = "12345678909",
                    BirthDate = new DateTime(1980, 1, 1),
                    CityCode = 5
                },
            };
        }
    }
}
