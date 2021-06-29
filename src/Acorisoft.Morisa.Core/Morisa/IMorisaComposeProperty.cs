namespace Acorisoft.Morisa
{
    public interface IMorisaComposeProperty : IDataProperty
    {
        //
        // TODO: Add Properties
    }


    public class MorisaComposeProperty : IMorisaComposeProperty
    {
        public static bool Validate(MorisaComposeProperty property)
        {
            // TODO: add validate methods
            return true;
        }
        public string Name { get; set; }
    }
}