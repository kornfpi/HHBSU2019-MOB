using System;
using System.Collections.Generic;
using System.Text;

namespace HabitatBuddy.Models
{
    public class DecisionTreeNode
    {
        public DecisionTreeNode parent;
        public List<DecisionTreeNode> children;
        public string questionText;
        public string answerText;
        public bool isRoot;
        public TodoREST.HomeIssue actionPlan;
        public string icon;

        public DecisionTreeNode()
        {
            parent = null;
            children = new List<DecisionTreeNode>();
            questionText = "blank question";
            answerText = "blank answer";
            actionPlan = null;
            isRoot = false;
            icon = "stoveoven70.png";
        }

    }
}
