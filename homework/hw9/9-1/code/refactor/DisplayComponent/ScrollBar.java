public class ScrollBar implements DisplayComponent{
    private boolean withScrollBar = false; //defalut as not show
    public ScrollBar(boolean b){
        withScrollBar = b;
    }
    public ScrollBar(){}
    public void setScrollBar(boolean b){
        withScrollBar = b;
    }
    public void show(){
        if(withScrollBar){ 
            // show scrollbar
        }
    }
}
