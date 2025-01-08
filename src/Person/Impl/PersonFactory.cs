namespace Person
{
    public class PersonFactory
    {
        private readonly ICanPersist persister;
        private readonly ICanConvertPerson personConverter;

        public PersonFactory(ICanPersist persister, ICanConvertPerson personConverter)
        {
            this.persister = persister;
            this.personConverter = personConverter;
        }


        // somewhere datei oder www, somewhat xml oder json
        public Person LoadFromSomewhereAsSomewhat(string persistentLocationPathOrWWW)
        {
            var srcString = persister.Load(persistentLocationPathOrWWW);
            var result = personConverter.FromString(srcString);

            return result;
        }
    }
}
