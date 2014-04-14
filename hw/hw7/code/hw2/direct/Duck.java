public class Duck{
    int doWhat;
    boolean canFly;
    public Duck(boolean _canFly, int _doWhat){
        canFly = _canFly;
        doWhat = _doWhat;
    }
    public void doAll(){
        display();
        swim();
        fly();
        sound();
    }
    public void display(){

    }
    public void swim(){
        System.out.println("Duck: swim");
    }
    public void fly(){
        if(!canFly)
            return;
        System.out.println("Duck: fly with wings");
    }
    public void sound(){
        switch(doWhat){
            case 1:    // quack
                System.out.println("Duck: quack");
                break;
            case 2:    // squeak
                System.out.println("Duck: squeak");
                break;
            case 3:    // silent
                System.out.println("Duck:");
                break;
        }
    }
    public void modifyFly(boolean _canFly){
        canFly = _canFly;
    }
    public void modifyQuack(int _doWhat){
        doWhat = _doWhat;
    }
    
}
