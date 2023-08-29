using System;

public interface IClosable
{
    public event Action Closed;
}