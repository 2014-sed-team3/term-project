public class TextView{
    public DisplayComponent[] component;
    public Format format;
    public TextView(int length, Format _format){
        component = new DisplayComponent[length];
        format = _format;
    }
    public void display(String path){
        String text = format.read(path);
        //display text
        for(int i=0;i<component.length;i++)
            if(component[i] != null)
                component[i].show();
    }
}
