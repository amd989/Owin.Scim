﻿namespace Owin.Scim
{
    using Model;

    public delegate void CanonicalizationRule<in T>(T attribute, ref object state) where T : MultiValuedAttribute;
}