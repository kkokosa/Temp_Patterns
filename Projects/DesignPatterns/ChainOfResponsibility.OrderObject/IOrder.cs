﻿namespace ChainOfResponsibility.OrderObject
{
    public interface IOrder
    {
        int Id { get; set; }
        decimal Price { get; set; }
    }
}