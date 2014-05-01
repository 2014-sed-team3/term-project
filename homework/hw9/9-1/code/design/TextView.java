public class TextView{
    public ScrollBar[] scrollbars = new ScrollBar[8];
    public BlackBorder border;
    public String text;

    public void display(){
        //display content
        //
        for(int i=0;i<scrollbars.length;i++){
            scrollbars[i].show();
        }
        if(border != null){
            border.show();
        }
    }
    public void formatTransform(String path, String typeStr){
        int type = identifyType(typeStr);
        switch(type){
            //case (type is type1):
            //  content = readformat(path);
            //  break;
            //case (type is type2):
            //  content = readformat(path);
            //  break;
        }
    }
    private int identifyType(String s){ return 0;}
}
