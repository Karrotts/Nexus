namespace Nexus;

public interface ISelectableList
{
    public List<String> GetOptions();
    public int GetIndexOfSelected();
    public void SetSelected(String value);
    public void SetSelected(int value);
    public String GetSelected();
}