import classes from './Footer.module.scss'
import MainButton from '../MainButton/MainButton'

interface FooterProps {
    mainButtonText: string
    mainButtonOnClick: () => void;
    visible: boolean;
}

export default function Footer({ mainButtonText, mainButtonOnClick, visible }: FooterProps) {
  const styles = visible ? '' : classes.hidden;

  return (
    <footer className={styles}>
        <MainButton 
            text={mainButtonText}
            onClick={mainButtonOnClick} 
        />
    </footer>
  )
}
