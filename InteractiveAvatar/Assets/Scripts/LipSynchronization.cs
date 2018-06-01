public class LipSynchronization
{
    private static LipSynchronization instance;
    
    private LipSynchronization() {}
    
    /// <summary>
    /// The initiation of the singleton: either returns the instance of it already exists and creates an instantiates
    /// an instance otherwise.
    /// </summary>
    public static LipSynchronization getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new LipSynchronization();
            }
            return instance;
        }
    }

    public void synchronize(string text)
    {
        // Text to phonemes and possible phonemes to visemes call
        
    }
}