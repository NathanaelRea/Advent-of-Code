use std::collections::HashSet;

fn main(){
    let ints: Vec<i32> = include_str!("../input.txt")
        .lines()
        .map(|n| n.parse::<i32>().unwrap())
        .collect();
        
    let mut set = HashSet::new();
    for i in ints {
        let j = 2020 - i;
        if set.contains(&j) {
            println!("{}", i * j)
        }
        set.insert(i);
    }
}