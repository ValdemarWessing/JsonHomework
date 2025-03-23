using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct GroupMembers
{
    public string name;
    public string favoriteColor;
    public string birthDay;

    public GroupMembers(string name,string birthDay, string favoriteColor)
    {
        this.name = name;
        this.favoriteColor = favoriteColor;
        this.birthDay = birthDay;
    }
}

[Serializable]
public class GroupMembersWrapper
{
    public List<GroupMembers> groupMembers;
}