# **Cat Breeding Genetic Algorithm Simulator**

A rule-based genetic algorithm simulation designed to model and analyze selective animal breeding strategies over multiple generations. This C\#/.NET project provides a framework for exploring the dynamics of polygenic trait inheritance, selection pressure, and the resulting evolution of a population's fitness.

## **1\. Motivation**

Selective breeding is a cornerstone of agriculture and animal husbandry, yet predicting its outcomes can be complex, especially for traits influenced by hundreds of genes (polygenic traits), such as intelligence, height, or specific performance metrics.

This simulator was created to:

* **Model Genetic Progress:** Quantify how quickly a population's average "fitness" can improve under a specific, rule-based breeding strategy.  
* **Analyze Strategies:** Provide a flexible platform to test and compare different breeding strategies (e.g., elitist selection, line breeding, outcrossing) by implementing the IBreedingStrategy interface.  
* **Understand Statistical Nuances:** Serve as an educational tool to demonstrate key statistical concepts in genetics, such as the Central Limit Theorem's effect on trait distribution and the importance of correcting for it.  
* **Provide a Foundation:** Act as a robust, test-driven core that can be extended with more complex biological models or a user interface for interactive simulations.

The core use case is to simulate a breeding program for a complex trait (analogous to IQ) and observe the statistical distribution of this trait in the population over time.

## **2\. The Genetic & Statistical Model**

The simulation is built on a carefully designed statistical foundation to ensure realistic and predictable behavior.

### **a) The Genetic Representation**

An individual animal (a Cat) is defined by a vector of numerical values, representing its genetic makeup.

* **Traits Vector:** Each cat has a TraitVector containing n floating-point values (e.g., 1280 traits). For a baseline "average" population, each trait value is randomly drawn from a Gaussian (Normal) distribution with a mean of 0.0.  
* **Fitness:** A specific complex trait we are breeding for (e.g., intelligence) is defined as the **fitness** of the cat. This is not a single gene but is calculated as the **average of a subset of k traits** from the main vector (e.g., the average of the first 640 traits).  
* **Fitness Quotient:** To make fitness values intuitive, the raw fitness score is converted to a standardized quotient, analogous to an IQ score. This is achieved by comparing the raw fitness to the theoretical global population, which has a defined mean of 0.0 and a standard deviation of 1.0.

### **b) The Core Statistical Challenge & Correction**

A naive implementation of the model above would lead to unrealistic results. This is due to a fundamental statistical principle: the **Central Limit Theorem**.

**The Problem:** The theorem states that the standard deviation of a sample mean is √N times *smaller* than the standard deviation of the individual data points. In our model:

* We generate each of the k sub-traits with a standard deviation of σ\_sub-trait.  
* Our fitness is the mean of these k sub-traits.  
* Therefore, the standard deviation of our fitness score would naturally become σ\_fitness \= σ\_sub-trait / √k.

If we set σ\_sub-trait \= 1.0 and k \= 640, the resulting fitness standard deviation would be 1.0 / √640 ≈ 0.04. This would create a population where almost everyone is extremely average, which is not realistic and severely dampens the effect of selective pressure.

**The Solution:** To counteract this, we **dynamically adjust** the standard deviation used to generate the initial sub-traits. The simulation engine calculates an adjusted standard deviation to ensure that the *final fitness score* has a standard deviation that matches our target (defined as 1.0).

The formula used is:  
σ\_adjusted\_sub-trait \= σ\_target\_fitness \* √k  
By pre-calculating this \_adjustedInitialSubTraitStdDev in the SimulationEngine, we ensure that the initial population's fitness is correctly distributed with a standard deviation of 1.0, regardless of how many sub-traits are used to calculate it. This makes the model robust and statistically sound.

[Obrázek: a normal distribution curve](https://encrypted-tbn0.gstatic.com/licensed-image?q=tbn:ANd9GcRjruak2iS-JVfEVJOoxWAnWi-vleydSU1hUwuFddjTBMqpvZ_NnFJYcFw82fod0nIsEdjmp-lBv6sPjwyB0lHDkEAHjHI-TUI-ftngysS4XOnFDS4)

Caption: The model ensures the initial population's fitness follows a standard Normal distribution (mean=0, stddev=1), providing a realistic baseline for the simulation.

## **3\. The Simulation Loop**

The simulation progresses in discrete time steps, moving from one generation to the next. The SimulationEngine orchestrates this entire process.

1. **Initialization:**  
   * A pool of y random candidates is generated (InitialPopulationSelectionPoolSize).  
   * The fitness of each candidate is calculated.  
   * The top x candidates (InitialPopulationSize) are selected to form the founding "Generation 0".  
2. **Mortality:**  
   * At the start of each new cycle, a simple mortality model is applied. Each cat has a probability of dying based on a configured "half-life" (MortalityHalfLife).  
3. **Breeding Strategy Application:**  
   * The core logic is delegated to a pluggable strategy that implements IBreedingStrategy. The default is ElitistBreedingStrategy.  
   * **Selection:** The strategy identifies the most suitable parents from the fertile population (e.g., the top MaxBreedingMales and MaxBreedingFemales based on fitness).  
   * **Pairing:** The selected parents are paired for breeding. The strategy includes fallback mechanisms, such as generating a random founder if no suitable males or females are available.  
4. **Inheritance and Mutation:**  
   * A litter of new kittens is generated for each mated female.  
   * For each trait, an offspring inherits the value from either its mother or father, chosen randomly.  
   * A small Gaussian mutation (InheritanceMutationStdDev) is added to the inherited trait value, introducing new genetic variation into the population.  
5. **Statistics Calculation:**  
   * After a new generation is born, the simulation calculates key statistics for that generation **only** (i.e., just the newborns). This provides a clear, un-skewed measure of genetic progress, avoiding the "parental inertia effect" where the high fitness of parents would otherwise mask the performance of the new generation.  
   * Metrics include: average/min/max fitness, fitness standard deviation, and population size.  
6. **Repeat:**  
   * The loop continues for the configured number of generations (TotalGenerationsToSimulate). The final result is a FinalReport object containing the statistics for each generation.

## **4\. Architecture & Usage**

The project is structured to be testable, flexible, and extensible.

### **Key Components**

* **SimulationEngine:** The orchestrator that runs the simulation loop.  
* **BreedingSimulationOptions:** A single class that holds all configuration parameters for a simulation run.  
* **IBreedingStrategy:** An interface for defining selection and breeding logic.  
  * **ElitistBreedingStrategy:** The default implementation that selects the highest-fitness individuals.  
* **IFitnessCalculator:** An interface for calculating the fitness of a cat.  
* **Domain Models:** Cat, Cattery, TraitVector represent the core entities.  
* **Analysis:** FitnessConverter and statistical classes like GenerationStatistics.

### **How to Run a Simulation**

// 1\. Configure the simulation  
var options \= new BreedingSimulationOptions  
{  
    TotalGenerationsToSimulate \= 50,  
    InitialPopulationSize \= 10,  
    InitialPopulationSelectionPoolSize \= 100,  
    MaxBreedingFemales \= 5,  
    InheritanceMutationStdDev \= 0.1  
};

// 2\. Instantiate the required services  
// (In a real application, this would be handled by a DI container)  
var randomProvider \= new RealRandomProvider(); // Implements IRandomProvider  
var fitnessCalculator \= new MeanTraitFitnessCalculator(options);  
var breedingStrategy \= new ElitistBreedingStrategy(options, fitnessCalculator, randomProvider);

// 3\. Create and run the simulation engine  
var engine \= new SimulationEngine(options, fitnessCalculator, breedingStrategy, randomProvider);  
FinalReport report \= engine.RunSingleSimulation();

// 4\. Analyze the results  
foreach (var stats in report.StatisticsByGeneration)  
{  
    Console.WriteLine($"Gen {stats.GenerationNumber}: Avg Fitness \= {stats.AverageFitness:F2}");  
}

## **5\. Building and Testing**

The solution is a standard .NET project.

* **Build:** dotnet build  
* **Test:** The project includes a comprehensive suite of MSTest unit tests that cover everything from individual component logic to full integration tests of the SimulationEngine. The tests were crucial in identifying and correcting the statistical models. Run with dotnet test.

## **6\. Future Work**

* **Advanced Breeding Strategies:** Implement other strategies like line breeding or outcrossing to avoid inbreeding depression.  
* **Genetic Linkage:** Model the concept of genes being physically close on a chromosome and thus more likely to be inherited together.  
* **UI Frontend:** Build a simple Blazor or MAUI application to visualize the simulation results with charts and graphs.
