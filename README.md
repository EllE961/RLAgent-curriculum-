# Unity ML-Agents Coin Collector

This Unity project showcases a reinforcement learning (RL) agent designed to navigate an environment, collect coins, and avoid obstacles using Unity's ML-Agents Toolkit. The agent is trained using reinforcement learning principles, with rewards provided for successful coin collection and penalties applied for hitting obstacles or walls.

## Project Overview

In this project, the environment consists of randomly placed coins and obstacles. The agent learns through trial and error to maximize its reward by:

- Collecting coins scattered across the environment.
- Avoiding obstacles to prevent penalties.
- Navigating the area with ray perception sensors to gather observations about its surroundings.

### Features

- **Reinforcement Learning Setup**: The agent uses discrete actions to move forward, backward, and rotate. It receives rewards for collecting coins and penalties for hitting obstacles or walls.
- **Dynamic Environment**: The positions of coins and obstacles are randomized in each episode, ensuring the agent learns in a non-static environment.
- **Ray Perception Sensor**: A 3D ray perception sensor helps the agent detect obstacles, walls, and coins in its vicinity, aiding its decision-making process.
- **Reset Mechanism**: At the start of each episode, the environment is reset, and the agent is placed at a new random starting position, promoting generalization in learning.
- **Particle Effects**: Visual feedback in the form of particle effects is triggered when the agent collects coins or hits obstacles.

### Agent Training

The agent is trained using Unity's ML-Agents Toolkit, where the agent interacts with the environment and updates its behavior through a reward system:
- **Positive Reward**: +1 for successfully collecting a coin.
- **Negative Reward**: -1 for hitting an obstacle or a wall.
- **Time Penalty**: The agent receives a small negative reward proportional to the length of the episode to encourage faster task completion.

### Dependencies

- **Unity ML-Agents**: Required to run the agent training and inference.
- **Unity 2021.3.0f1 or later**: Recommended Unity version for optimal performance.
