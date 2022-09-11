use std::collections::HashMap;

fn main() {
    let ints: Vec<i32> = include_str!("../input.txt")
        .lines()
        .map(|n| n.parse().unwrap())
        .collect();

    let mut map = HashMap::new();
    for i in 0..ints.len()-2 {
        for j in i+1..ints.len()-1 {
            map.insert(ints[i] + ints[j], (ints[i], ints[j]));
        }
    }

    for i in ints {
        let to_find = 2020 - i;
        if map.contains_key(&to_find) {
            let (j, k) = map.get(&to_find).unwrap();
            println!("{}", i * j * k);
            return;
        }
    }
}
