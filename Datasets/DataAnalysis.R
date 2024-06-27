library(tidyverse)

# Load dataset from CSV and display parameters; Replace path with path to dataset
primary_scl_data = read_csv("C:/Users/asima/Documents/University/Year 3/Individual Project/Datasets/primarysclattendance.csv")
colnames(primary_scl_data)

primary_scl_data %>%
  ggplot(aes(x=Attendance)) +
    geom_histogram(binwidth=0.01, color="black") +
    labs(x="Attendance", y="Frequency", 
         title="Attendance Rate Distribution")

secondary_scl_data = read_csv("C:/Users/asima/Documents/University/Year 3/Individual Project/Datasets/secondarysclattendance.csv")
colnames(secondary_scl_data)

secondary_scl_data %>%
  ggplot(aes(x=Attendance)) +
  geom_boxplot() +
  labs(x="Attendance", y="Frequency", 
       title="Attendance Rate Distribution")





'''# AvgScore per level line chart
arcade_data %>%
  # Find average score by level
  group_by(Level) %>%
  summarize(AverageScore=mean(Score)) %>%
  # Plot average score by level
  ggplot(aes(x=Level, y=AverageScore)) +
  # As a line chart
  geom_line() +
  # Use appropriate labels
  labs(x="Level", y="Average Score", 
       title="Average Score for each Level")

# Score-Accuracy Scatter plot colour-coded by Wins and Losses
arcade_data %>%
  # Plot score by accuracy, colour-coded by wins and losses
  ggplot(aes(x = Score, y = Accuracy, color = IsWin)) +
  # As a scatter plot
  geom_point() +
  # Use appropriate labels
  labs(x="Score", y="Accuracy",
       title="Scatter Plot of Accuracy vs. Score, Showing Wins and Losses")

# Shows AvgScore categorised by Wins and Losses
arcade_data %>%
  # Categorising by wins and losses
  group_by(IsWin) %>%
  # Calculate mean score
  summarize(AverageScore = mean(Score)) %>%
  # Plot avgscore by wins and losses
  ggplot(aes(x = IsWin, y = AverageScore, fill = IsWin)) +
  # As a column chart
  geom_col() +
  # Use appropriate labels
  labs(x="Is Win", y="Average Score",
       title="Average Score by Wins and Losses") '''

