namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    class SimpleTestValueObject : ValueObject
    {
        public SimpleTestValueObject(string name, int age)
        {
            Name = name;
            Age = age;
        }

        [NameValidation]
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
