namespace slushiecorp.Enums
{ 
    public enum CustomerStates
    {
        New = 0,
        Consuming = -1,
        Ordering = 1,
        Rejected = -10
    }

    public enum OrderStates
    {
        New = 0,
        Accepted = 10,
        Rejected = -10
    }
}