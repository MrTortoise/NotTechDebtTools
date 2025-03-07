use std::io;
use rand::Rng;
use std::cmp::Ordering;

fn main() {
    println!("Hello, world!");

    println!("Guess the number!");
    let secret_number = rand::rng().random_range(1..=100);
    // println!("The secret number is: {secret_number}");

    let x = 5;
    println!("The value of x is: {x}");
    x = 6;
    println!("The value of x is: {x}");

}
