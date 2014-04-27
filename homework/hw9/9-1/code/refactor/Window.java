public class Window{
    public TextView t;
    public String path;
    public Window(Format f){
        t = new TextView(100, f);
    }
    public void show(){
        t.display(path);
    }
}
