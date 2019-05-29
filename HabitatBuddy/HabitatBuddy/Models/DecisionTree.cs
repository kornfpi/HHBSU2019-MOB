using System;

namespace HabitatBuddy.Models {
    public class DecisionTree {
        private DecisionTreeNode root;
        private DecisionTreeNode cursor;

        public DecisionTree(string rootQuestion) {
            root = new DecisionTreeNode();
            root.isRoot = true;
            root.questionText = rootQuestion;
            cursor = root;
        }

        public void moveToChild(int index) {
            cursor = cursor.children[index];
        }

        public void moveToParent() {
            if(!cursor.isRoot) { // Move to parent unless cursor is at the root which has no parent
                cursor = cursor.parent;
            }
        }

        public void addChild(string questionText, string answerText,TodoREST.HomeIssue actionPlan) {
            DecisionTreeNode child = new DecisionTreeNode();
            child.parent = cursor;
            child.questionText = questionText;
            child.answerText = answerText;
            child.actionPlan = actionPlan;
            cursor.children.Add(child);
        }

        public int getNumChildren() {
            return cursor.children.Count;
        }

        public string getQuestionText() {
            return cursor.questionText;
        }

        public string getAnswerText() {
            return cursor.answerText;
        }

        public TodoREST.HomeIssue getActionPlan() {
            return cursor.actionPlan;
        }
    }
}