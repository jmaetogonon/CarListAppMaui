﻿namespace CarListAppApi
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Vin { get; set; } = string.Empty;
    }
}