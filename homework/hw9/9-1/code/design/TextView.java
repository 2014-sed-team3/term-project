public class TextView{
    public int windowSize = 0;
    public int contentSize = 0;
    public boolean withScrollBar = false;
    public int blackBorderSize = 0;
    public String Content;

    public void display(){
        //display content
        //
        if(withScrollBar){
            showScrollBar();
        }
        if(blackBorderSize > 0){
            showBlackBorder();
        }
    }
    public void setBlackBorder(int size){ blackBorderSize = size; }
    public void setScrollBar(boolean b){ withScrollBar = b;  }
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
    private void showScrollBar(){}
    private void showBlackBorder(){}
    private int identifyType(String s){ return 0;}
}
