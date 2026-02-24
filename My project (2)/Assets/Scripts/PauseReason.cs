/* PauseReason.cs holds an enum for valid pause reasons. this reduces
 * Spelling-based errors (e.g. calling request pause with "Inventory" and 
 * release pause )
*/
public enum PauseReason
{
    None        = 0, // basically a null value for this enum, we assign it 
                     // manually bc c# will automatically assign it if we don't
    Inventory   = 1,
    PauseMenu   = 2,

}