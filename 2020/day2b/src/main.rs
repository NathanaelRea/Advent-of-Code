use text_io::scan;

fn main() {
    let input = include_str!("../input.txt");
    
    let mut n_valid_passwords = 0;
    for line in input.lines() {
        let first: usize;
        let second: usize;
        let letter: char;
        let password: String;
        scan!(line.bytes() => "{}-{} {}: {}", first, second, letter, password);

        if (password.chars().nth(first-1).unwrap() == letter) ^
            (password.chars().nth(second-1).unwrap() == letter){
            n_valid_passwords += 1
        }
    }
    println!("{}", n_valid_passwords)
}
